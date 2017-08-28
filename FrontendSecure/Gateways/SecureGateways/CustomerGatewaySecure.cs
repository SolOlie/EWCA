using System;
using System.Collections.Generic;
using System.Web;
using Entities.Entities;

namespace FrontendSecure.Gateways.SecureGateways
{
    public class CustomerGatewaySecure : IServiceGateway<Customer>
    {
        public Customer Create(Customer t)
        {
            var customer = WebapiService.instance.PostAsync<Customer>("/api/ApiCustomers/PostCustomer", t, HttpContext.Current.User.Identity.Name).Result;
            return customer;
        }

        public Customer Read(int id)
        {
            var customer = WebapiService.instance.GetAsync<Customer>("/api/ApiCustomers/GetCustomer/"+id, HttpContext.Current.User.Identity.Name).Result;
            return customer;
        }

        public List<Customer> ReadAll()
        {
            var customers = WebapiService.instance.GetAsync<List<Customer>>("/api/ApiCustomers/GetCustomers", HttpContext.Current.User.Identity.Name).Result;
            return customers;
        }

        public bool Delete(Customer t)
        {
            var customer = WebapiService.instance.DeleteAsync<Customer>("/api/ApiCustomers/DeleteCustomer/" + t.Id, HttpContext.Current.User.Identity.Name).Result;
            return customer;
        }

        public bool Update(Customer t)
        {
            var customer = WebapiService.instance.PutAsync("/api/ApiCustomers/PutCustomer/" + t.Id ,t, HttpContext.Current.User.Identity.Name).Result;
            return customer;
        }

        public List<Customer> ReadAllWithFk(int id)
        {
            throw new NotImplementedException();
        }
    }
}