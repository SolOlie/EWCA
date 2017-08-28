using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using DAL.DB;
using Entities.Entities;

namespace DAL.Repositories
{
    class UserRepo : IRepository<User>
    {
        public User Create(User t)
        {
            using (var ctx = new CADBContext())
            {
                User u = null;
                if (ctx.Users.Count(x => x.Email == t.Email) < 1)
                {
                    var cryptoSave = new Crypto();
                    t.Password = cryptoSave.Encrypt(t.Password);
                    ctx.Entry(t.IsContactForCustomer).State = EntityState.Unchanged;
                    u = ctx.Users.Add(t);
                    ctx.SaveChanges();
                }
                
                return u;
            }
        }

        public User Read(int id)
        {
            using (var ctx = new CADBContext())
            {
                var o = ctx.Users.Include(x => x.Changelogs).Include(x => x.IsContactForCustomer).FirstOrDefault(x => x.Id == id);
                o.Password = new Crypto().Decrypt(o.Password);
                return o;
            }
        }

        public List<User> ReadAll()
        {
            using (var ctx = new CADBContext())
            {
                var o = ctx.Users.Include(x => x.Changelogs).Include(x=> x.IsContactForCustomer).ToList();
                foreach (var u in o)
                {
                    u.Password = new Crypto().Decrypt(u.Password);
                }
                return o;
            }
        }

        public bool Delete(User t)
        {
            using (var ctx = new CADBContext())
            {
                if (UserExists(t.Id, ctx))
                {
                    ctx.Users.Attach(t);
                    ctx.Users.Remove(t);
                    ctx.SaveChanges();
                    return true;
                }
                return false;

            }
        }

        private bool UserExists(int id, CADBContext ctx)
        {
            if (ctx.Users.Count(a => a.Id == id) > 0)
            {
                return true;
            }
            return false;
        }

        public bool Update(User t)
        {
            using (var ctx = new CADBContext())
            {
                var cryptoSave = new Crypto();
                t.Password = cryptoSave.Encrypt(t.Password);
                ctx.Entry(t).State = EntityState.Modified;
                try
                {
                    ctx.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(t.Id, ctx))
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


        public List<User> ReadAllWithFk(int id)
        {
            using (var ctx = new CADBContext())
            {
                var o = ctx.Users.Include(x => x.Changelogs).Include(x => x.IsContactForCustomer).Where(x => x.IsContactForCustomer.Id == id).ToList();
                foreach (var u in o)
                {
                    u.Password = new Crypto().Decrypt(u.Password);
                }
                return o;
            }
        }
     
    }
}
