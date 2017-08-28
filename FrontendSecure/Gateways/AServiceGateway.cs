using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;

namespace FrontendSecure.Gateways
{
    public abstract class AServiceGateway<T> : IServiceGateway<T>
    {
        public T Create(T t)
        {
            using (var client = new HttpClient())
            {
                Setup(client);

                t = CreatePost(t, client);
                
                    return t;
            }
        }

        protected abstract T CreatePost(T t , HttpClient client);


        public bool Delete(T t)
        {
            using (var client = new HttpClient())
            {
                Setup(client);

               bool isDel = DeleteDel(t, client);

                return isDel;
            }
        }

        protected abstract bool DeleteDel(T t, HttpClient client);

        public T Read(int id)
        {
            using (var client = new HttpClient())
            {
                Setup(client);

                T t = ReadOne(id, client);

                return t;
            }
        }

        protected abstract T ReadOne(int id, HttpClient client);

        public List<T> ReadAll()
        {
            using (var client = new HttpClient())
            {
                Setup(client);

                List<T> t = ReadAllRead(client);

                return t;
            }
        }

        protected abstract List<T> ReadAllRead(HttpClient client);

        public List<T> ReadAllWithFk(int id)
        {
            using (var client = new HttpClient())
            {
                Setup(client);

               List<T> t =ReadAllWithFkRead(id, client);

                return t;
            }
        }

        protected abstract List<T> ReadAllWithFkRead(int id, HttpClient client);

        public bool Update(T t)
        {
            using (var client = new HttpClient())
            {
                Setup(client);

                bool b = UpdatePut(t, client);

                return b;
            }
        }

        protected abstract bool UpdatePut(T t, HttpClient client);

        private HttpClient Setup(HttpClient client)
        {
            client.BaseAddress = new Uri("http://ewcustomeraccountingbackend20170828013231.azurewebsites.net/EWCustomerAccountingBackend");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            if (HttpContext.Current.Session["token"] != null)
            {
                string token = HttpContext.Current.Session["token"].ToString();
                client.DefaultRequestHeaders.Remove("Authorization");
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            }
            return client;


        }


    }
}
