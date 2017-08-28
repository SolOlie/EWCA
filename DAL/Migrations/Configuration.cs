using System.Collections.Generic;
using Entities.Entities;

namespace DAL.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<DAL.DB.CADBContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "DAL.DB.CADBContext";
        }

        protected override void Seed(DAL.DB.CADBContext context)
        {
            //List<AssetType> assetTypes = new List<AssetType>();
            //List<Asset> assets = new List<Asset>();
            //List<User> users = new List<User>();
            //List<Customer> customers = new List<Customer>();
            //List<Changelog> changelogs = new List<Changelog>();


            //assetTypes.Add(new AssetType()
            //{
            //    Description = "switch"
            //});
            //assetTypes.Add(new AssetType()
            //{
            //    Description = "Server"
            //});
            //assetTypes.Add(new AssetType()
            //{
            //    Description = "Firewall"
            //});


            //customers.Add(new Customer()
            //{
            //    Firm = "EliteWeb",
            //    Date = DateTime.Now,
            //    Address = "Vesterhavsgade 139",

            //});
            //customers.Add(new Customer()
            //{
            //    Firm = "Spejdersport",
            //    Date = DateTime.Now,
            //    Address = "Kongensgade ved siden af Humac"
            //});
            //for (int i = 0; i < 10; i++)
            //{
            //    users.Add(new User()
            //    {
            //        Email = "ag@ew" + i + ".dk",
            //        FirstName = "Anders " + i,
            //        LastName = "Gadeberg" + i,
            //        Password = "Test" + i,
            //        PhoneNumber = "12345678",
            //        IsContactForCustomer = customers[i % 2]
            //    });
            //}
            //for (int i = 0; i < 30; i++)
            //{
            //    assets.Add(new Asset()
            //    {
            //        Name = "Server: " + i,
            //        Type = assetTypes[i % 3],
            //        Customer = customers[i % 2],
            //        Password = "asset" + i,
            //        InstallationDate = DateTime.Now,
            //        Address = "Havnen",
            //        Description = "A damn fine server",
            //        Location = "Esbjerg",
            //        OS = "Windows 98 ME",
            //        Usedby = "Testensen",
            //        IpAddress = "127.0.0.1",
            //        Login = "Eliteweb",
            //    });
            //}
            //for (int i = 0; i < 100; i++)
            //{
            //    changelogs.Add(new Changelog()
            //    {
            //        ChangedDate = DateTime.Now,
            //        Description = "This assets was fixed" + i,
            //        User = users[0],
            //        Hours = 10,
            //        Asset = assets[i % 30]
            //    });
            //}


            //foreach (var assetType in assetTypes)
            //{
            //    context.AssetTypes.Add(assetType);
            //}
            //foreach (var c in customers)
            //{
            //    context.Customers.Add(c);
            //}
            //foreach (var asset in assets)
            //{
            //    asset.Password = new Crypto().Encrypt(asset.Password);
            //    context.Assets.Add(asset);
            //}
            //foreach (var user in users)
            //{
            //    user.Password = new Crypto().Encrypt(user.Password);
            //    context.Users.Add(user);
            //}
            //foreach (var chanelogs in changelogs)
            //{
            //    context.Changelogs.Add(chanelogs);
            //}
            //context.SaveChanges();
        }
    }
}
