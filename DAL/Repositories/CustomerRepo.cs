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
    class CustomerRepo : IRepository<Customer>
    {
        public Customer Create(Customer t)
        {
            using (var ctx = new CADBContext())
            {
                Customer a = ctx.Customers.Add(t);
                ctx.SaveChanges();
                return a;
            }
        }

        public Customer Read(int id)
        {
            using (var ctx = new CADBContext())
            {
                var customer = ctx.Customers.Include(y => y.Assets).Include(y => y.ContactPersons).FirstOrDefault(x => x.Id == id);
                foreach (var c in customer.ContactPersons)
                {
                    c.Password = new Crypto().Decrypt(c.Password);
                }
                foreach (var r in customer.Assets)
                {
                    r.Password = new Crypto().Decrypt(r.Password);
                }
                return customer;
            }
        }

        public List<Customer> ReadAll()
        {
            using (var ctx = new CADBContext())
            {
                var listCustomer = ctx.Customers.Include(y => y.Assets).Include(y => y.ContactPersons).ToList();
                foreach (var c in listCustomer)
                {
                    foreach (var u in c.ContactPersons)
                    {
                        u.Password = new Crypto().Decrypt(u.Password);
                    }
                    foreach (var r in c.Assets)
                    {
                        r.Password = new Crypto().Decrypt(r.Password);
                    }
                }
                return listCustomer;
            }
        }

        public bool Delete(Customer t)
        {
            using (var ctx = new CADBContext())
            {
                if (CustomerExists(t.Id, ctx))
                {
                    ctx.Customers.Attach(t);
                    ctx.Customers.Remove(t);
                    ctx.SaveChanges();
                    return true;
                }
                return false;
            }
        }

        private bool CustomerExists(int tId, CADBContext ctx)
        {
            if (ctx.Customers.Count(a => a.Id == tId) > 0)
            {
                return true;
            }
            return false;
        }

        public bool Update(Customer t)
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
                    if (!CustomerExists(t.Id, ctx))
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

        public List<Customer> ReadAllWithFk(int id)
        {
            throw new NotImplementedException();
        }

       
    }
}
