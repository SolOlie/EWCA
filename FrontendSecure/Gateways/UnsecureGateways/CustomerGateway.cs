using System;
using System.Collections.Generic;
using System.Net.Http;
using Entities.Entities;

namespace FrontendSecure.Gateways.UnsecureGateways
{
    class CustomerGateway: AServiceGateway<Customer>
    {
        protected override Customer CreatePost(Customer t, HttpClient client)
        {
            HttpResponseMessage response = client.PostAsJsonAsync("api/ApiCustomers/PostCustomer", t).Result;
            if (response.IsSuccessStatusCode)
            {
                return response.Content.ReadAsAsync<Customer>().Result;
            }
            return null;
        }

        protected override bool DeleteDel(Customer t, HttpClient client)
        {
            var response = client.DeleteAsync("/api/ApiCustomers/DeleteCustomer/" + t.Id).Result;
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }

        protected override Customer ReadOne(int id, HttpClient client)
        {
            HttpResponseMessage response = client.GetAsync("api/ApiCustomers/GetCustomer/" + id).Result;
            if (response.IsSuccessStatusCode)
            {
                return response.Content.ReadAsAsync<Customer>().Result;
            }
            return null;
        }

        protected override List<Customer> ReadAllRead(HttpClient client)
        {
            HttpResponseMessage response = client.GetAsync("api/ApiCustomers/GetCustomers").Result;
            if (response.IsSuccessStatusCode)
            {
                return response.Content.ReadAsAsync<List<Customer>>().Result;
            }
            return new List<Customer>();
        }

        protected override List<Customer> ReadAllWithFkRead(int id, HttpClient client)
        {
            throw new NotImplementedException();
        }

        protected override bool UpdatePut(Customer t, HttpClient client)
        {
            var response = client.PutAsJsonAsync("api/ApiCustomers/PutCustomer/" + t.Id, t).Result;
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }
    }
}
