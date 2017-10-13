using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Entities.Entities;

namespace FrontendSecure.Gateways.SecureGateways
{
    public class CustomerFileGatewaySecure : IServiceGateway<CustomerFile>
    {
        public CustomerFile Create(CustomerFile t)
        {
            var CustomerFile = WebapiService.instance.PostAsync<CustomerFile>("/api/CustomerFiles/PostCustomerFile", t, HttpContext.Current.User.Identity.Name).Result;
            return CustomerFile;
        }

        public CustomerFile Read(int id)
        {
            var CustomerFile = WebapiService.instance.GetAsync<CustomerFile>("/api/CustomerFiles/GetCustomerFile/" + id, HttpContext.Current.User.Identity.Name).Result;
            return CustomerFile;
        }

        public List<CustomerFile> ReadAll()
        {
            var CustomerFiles = WebapiService.instance.GetAsync<List<CustomerFile>>("/api/CustomerFiles/GetCustomerFiles", HttpContext.Current.User.Identity.Name).Result;
            return CustomerFiles;
        }

        public bool Delete(CustomerFile t)
        {
            var CustomerFile = WebapiService.instance.DeleteAsync<CustomerFile>("/api/CustomerFiles/DeleteCustomerFile/" + t.Id, HttpContext.Current.User.Identity.Name).Result;
            return CustomerFile;
        }

        public bool Update(CustomerFile t)
        {
            var CustomerFile = WebapiService.instance.PutAsync<CustomerFile>("/api/CustomerFiles/PutCustomerFile/" + t.Id, t, HttpContext.Current.User.Identity.Name).Result;
            return CustomerFile;
        }

        public List<CustomerFile> ReadAllWithFk(int id)
        {
            var CustomerFile = WebapiService.instance.GetAsync<List<CustomerFile>>("/api/CustomerFiles/GetCustomerFilesWithFk/" + id, HttpContext.Current.User.Identity.Name).Result;
            return CustomerFile;
        }
    }
}