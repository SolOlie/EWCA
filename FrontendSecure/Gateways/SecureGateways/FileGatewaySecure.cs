using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Entities.Entities;

namespace FrontendSecure.Gateways.SecureGateways
{
    public class FileGatewaySecure: IServiceGateway<File>
    {
        public File Create(File t)
        {
            var File = WebapiService.instance.PostAsync<File>("/api/Files/PostFile", t, HttpContext.Current.User.Identity.Name).Result;
            return File;
        }

        public File Read(int id)
        {
            var File = WebapiService.instance.GetAsync<File>("/api/Files/GetFile/" + id, HttpContext.Current.User.Identity.Name).Result;
            return File;
        }

        public List<File> ReadAll()
        {
            var Files = WebapiService.instance.GetAsync<List<File>>("/api/Files/GetFiles", HttpContext.Current.User.Identity.Name).Result;
            return Files;
        }

        public bool Delete(File t)
        {
            var File = WebapiService.instance.DeleteAsync<File>("/api/Files/DeleteFile/" + t.Id, HttpContext.Current.User.Identity.Name).Result;
            return File;
        }

        public bool Update(File t)
        {
            var File = WebapiService.instance.PutAsync("/api/Files/PutFile/" + t.Id, t, HttpContext.Current.User.Identity.Name).Result;
            return File;
        }

        public List<File> ReadAllWithFk(int id)
        {
            var File = WebapiService.instance.GetAsync<List<File>>("/api/Files/GetFilesWithFk/" + id, HttpContext.Current.User.Identity.Name).Result;
            return File;
        }
    }
}