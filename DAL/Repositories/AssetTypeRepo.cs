using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.DB;
using Entities.Entities;

namespace DAL.Repositories
{
    class AssetTypeRepo: IRepository<AssetType>
    {
        public AssetType Create(AssetType t)
        {
            using (var ctx = new CADBContext())
            {
                AssetType a = ctx.AssetTypes.Add(t);
                ctx.SaveChanges();
                return a;
            }
        }

        public AssetType Read(int id)
        {
            using (var ctx = new CADBContext())
            {
                return ctx.AssetTypes.Include(m=>m.Assets).FirstOrDefault(x => x.Id == id);
            }
        }

        public List<AssetType> ReadAll()
        {
            using (var ctx = new CADBContext())
            {
                var c = ctx.AssetTypes.Include(m => m.Assets).ToList();
                 return c;
            }
        }

        public bool Delete(AssetType t)
        {
            using (var ctx = new CADBContext())
            {
                if (AssetTypeExists(t.Id, ctx))
                {
                    ctx.AssetTypes.Attach(t);
                    ctx.AssetTypes.Remove(t);
                    ctx.SaveChanges();
                    return true;
                }
                return false;


            }
        }

        private bool AssetTypeExists(int tId, CADBContext ctx)
        {
            if (ctx.AssetTypes.Count(a => a.Id == tId) > 0)
            {
                return true;
            }
            return false;

        }

        public bool Update(AssetType t)
        {
            using (var ctx = new CADBContext())
            {
                ctx.Entry(t).State = EntityState.Modified;
                try
                {
                    ctx.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AssetTypeExists(t.Id, ctx))
                    {
                        return false;
                    }
                    else
                    {
                        throw;
                    }
                }
                return true;
            }
        }

        public List<AssetType> ReadAllWithFk(int id)
        {
            throw new NotImplementedException();
        }
    }
}
