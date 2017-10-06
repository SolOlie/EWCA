using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Entities.Entities;

namespace FrontendSecure.Gateways.SecureGateways
{
    public class ManufacturerGatewaySecure : IServiceGateway<Manufacturer>
    {
        public Manufacturer Create(Manufacturer t)
        {
            var Manufacturer = WebapiService.instance.PostAsync<Manufacturer>("/api/Manufacturers/PostManufacturer", t, HttpContext.Current.User.Identity.Name).Result;
            return Manufacturer;
        }

        public Manufacturer Read(int id)
        {
            var Manufacturer = WebapiService.instance.GetAsync<Manufacturer>("/api/Manufacturers/GetManufacturer/" + id, HttpContext.Current.User.Identity.Name).Result;
            return Manufacturer;
        }

        public List<Manufacturer> ReadAll()
        {
            var Manufacturers = WebapiService.instance.GetAsync<List<Manufacturer>>("/api/Manufacturers/GetManufacturers", HttpContext.Current.User.Identity.Name).Result;
            return Manufacturers;
        }

        public bool Delete(Manufacturer t)
        {
            var Manufacturer = WebapiService.instance.DeleteAsync<Manufacturer>("/api/Manufacturers/DeleteManufacturer/" + t.Id, HttpContext.Current.User.Identity.Name).Result;
            return Manufacturer;
        }

        public bool Update(Manufacturer t)
        {
            var Manufacturer = WebapiService.instance.PutAsync("/api/Manufacturers/PutManufacturer/" + t.Id, t, HttpContext.Current.User.Identity.Name).Result;
            return Manufacturer;
        }

        public List<Manufacturer> ReadAllWithFk(int id)
        {
            throw new NotImplementedException();
        }
    }
}