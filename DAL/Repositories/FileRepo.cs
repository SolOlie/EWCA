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
    class FileRepo : IRepository<File>
    {
        public File Create(File t)
        {
            using (var ctx = new CADBContext())
            {
                ctx.Entry(t.Asset).State = EntityState.Unchanged;
                File a = ctx.Files.Add(t);

                ctx.SaveChanges();
                return a;
            }
        }

        public File Read(int id)
        {
            using (var ctx = new CADBContext())
            {
                return ctx.Files.Include(m => m.Asset).Include(m => m.ContentFile).FirstOrDefault(x => x.Id == id);
            }
        }

        public List<File> ReadAll()
        {
            using (var ctx = new CADBContext())
            {
                var c = ctx.Files.Include(m => m.Asset).ToList();
                return c;
            }
        }

        public bool Delete(File t)
        {
            using (var ctx = new CADBContext())
            {
                if (FileExists(t.Id, ctx))
                {
                    ctx.Files.Attach(t);
                    ctx.Files.Remove(t);
                    ctx.SaveChanges();
                    return true;
                }
                return false;


            }
        }

        private bool FileExists(int tId, CADBContext ctx)
        {
            if (ctx.Files.Count(a => a.Id == tId) > 0)
            {
                return true;
            }
            return false;

        }

        public bool Update(File t)
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
                    if (!FileExists(t.Id, ctx))
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

        public List<File> ReadAllWithFk(int id)
        {
            using (var ctx = new CADBContext())
            {
                var c = ctx.Files.Include(m => m.Asset).Where(x=> x.Asset.Id == id).ToList();
                return c;
            }
        }
    }
}
