using System;
using System.Net;
using System.Web.Mvc;
using Entities.Entities;
using FrontendSecure.Gateways;

namespace FrontendSecure.Controllers
{
    public class ChangelogsController : Controller
    {
        private IServiceGateway<Changelog> db = new BllFacade().GetChangelogGateway();
        [HttpPost]
        public bool ModifiedAdd(string Description, int userId,  double Hours, DateTime Date, int assetId, int? id)
        {
            if (Description == null)
            {
                return false;
            }
            db.Create(new Changelog()
            {
                Description = Description,
                User = new User()
                {
                  Id  = userId
                },
                Asset = new Asset()
                {
                    Id = assetId
                },
                Hours = Hours,
                ChangedDate = Date
            });
            return true;
        }
        [HttpPost]
        public bool ModifiedDelete(int id)
        {
            Changelog changelog = db.Read(id);
            var deleted = db.Delete(changelog);
            return deleted;
        }

        [HttpPost]
        public bool ModifiedEdit(string Description, int userId, double Hours, DateTime Date, int assetId, int id)
        {
            var changelog = db.Read(id);
            changelog.UserId = userId;
            changelog.User.Id = userId;
            changelog.Asset.Id = assetId;
            changelog.ChangedDate = Date;
            changelog.Description = Description;
            changelog.Hours = Hours;
            var updated = db.Update(changelog);
            return updated;
        }

    }
}
