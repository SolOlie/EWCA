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
    class CustomerFileRepo : IRepository<CustomerFile>
    {
        public CustomerFile Create(CustomerFile t)
        {
            using (var ctx = new CADBContext())
            {
                ctx.Entry(t.Customer).State = EntityState.Unchanged;
                CustomerFile a = ctx.CustomerFiles.Add(t);
                
                ctx.SaveChanges();
                return a;
            }
        }

        public CustomerFile Read(int id)
        {
            using (var ctx = new CADBContext())
            {
                return ctx.CustomerFiles.Include(m => m.CustomerContentType).FirstOrDefault(x => x.Id == id);
            }
        }

        public List<CustomerFile> ReadAll()
        {
            using (var ctx = new CADBContext())
            {
                var c = ctx.CustomerFiles.Include(m => m.Customer).ToList();
                return c;
            }
        }

        public bool Delete(CustomerFile t)
        {
            using (var ctx = new CADBContext())
            {
                if (FileExists(t.Id, ctx))
                {
                    ctx.CustomerFiles.Attach(t);
                    ctx.CustomerFiles.Remove(t);
                    ctx.SaveChanges();
                    return true;
                }
                return false;


            }
        }
        private bool FileExists(int tId, CADBContext ctx)
        {
            if (ctx.CustomerFiles.Count(a => a.Id == tId) > 0)
            {
                return true;
            }
            return false;

        }
        public bool Update(CustomerFile t)
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

        public List<CustomerFile> ReadAllWithFk(int id)
        {
            using (var ctx = new CADBContext())
            {
                var c = ctx.CustomerFiles.Include(m => m.Customer).Where(x => x.Customer.Id == id).ToList();
                return c;
            }
        }
    }
}
