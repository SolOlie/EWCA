using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.DB;
using Entities.Entities;
using TrackerEnabledDbContext.Common.Models;

namespace DAL.Repositories
{
    class AssetRepo : IRepository<Asset>
    {
        
        public Asset Create(Asset t)
        {
            using (var ctx = new CADBContext())
            {
                var cryptoSave = new Crypto();
                t.Password = cryptoSave.Encrypt(t.Password);
                t.CustomerId = t.Customer.Id;
                t.TypeId = t.Type.Id;
                Asset a = ctx.Assets.Add(t);
                ctx.Entry(t.Customer).State = EntityState.Unchanged;
                ctx.Entry(t.Type).State = EntityState.Unchanged;
                ctx.SaveChanges();
                return a;
            }
        }

        public Asset Read(int id)
        {
            using (var ctx = new CADBContext())
            {
                var a = ctx.Assets.Include(y => y.Customer).Include(y => y.Changelogs.Select(u => u.User)).Include(y => y.Type).Include(y=> y.FileAttachments).FirstOrDefault(x => x.Id == id);
                if(a!=null && a.Password?.Length >0)
                a.Password = new Crypto().Decrypt(a.Password);
                return a;
            }
        }

        public List<Asset> ReadAll()
        {
            using (var ctx = new CADBContext())
            {
                var aList = ctx.Assets.Include(y => y.Customer).Include(y => y.Changelogs).Include(y => y.Type).ToList();
                foreach (var s in aList)
                {
                    s.Password = new Crypto().Decrypt(s.Password);
                }
                return aList;
            }
        }

        public bool Delete(Asset t)
        {
            using (var ctx = new CADBContext())
            {
                if(AssetExists(t.Id,ctx))
                {
                    ctx.Assets.Attach(t);
                    ctx.Assets.Remove(t);
                    ctx.SaveChanges();
                    return true;
                }
                return false;


            }
        }

        public bool Update(Asset t)
        {
            using (var ctx = new CADBContext())
            {
               
                var cryptoSave = new Crypto();
                t.Password = cryptoSave.Encrypt(t.Password);
                t.CustomerId = t.Customer.Id;
                t.TypeId = t.Type.Id;
                ctx.Assets.Attach(t);
                ctx.Entry(t).State = EntityState.Modified;
                try
                {
                    ctx.SaveChanges(t.Id);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AssetExists(t.Id,ctx))
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

        public List<Asset> ReadAllWithFk(int id)
        {
            using (var ctx = new CADBContext())
            {
                var afk = ctx.Assets.Include(y => y.Customer).Include(y => y.Changelogs).Include(y => y.Type).Where(x => x.Customer.Id == id).ToList();
                foreach (var s in afk)
                {
                    s.Password = new Crypto().Decrypt(s.Password);
                }
                return afk;
            }
        }

        bool AssetExists(int id, CADBContext ctx)
        {
            if (ctx.Assets.Count(a => a.Id == id) > 0)
            {
                return true;
            }
            return false;
        }
    }
}
