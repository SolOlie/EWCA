using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Repositories;
using Entities.Entities;

namespace DAL
{
    public class Facade
    {
        public IRepository<Asset> GetAssetRepo()
        {
            return new AssetRepo();
        }

        public IRepository<Changelog> GetChangelogRepo()
        {
            return new ChangelogRepo();
        }

        public IRepository<Customer> GetCustomerRepo()
        {
            return new CustomerRepo();
        }

        public IRepository<User> GetUserRepo()
        {
            return new UserRepo();
        }
        public IRepository<AssetType> GetAssetTypeRepo()
        {
            return new AssetTypeRepo();
        }

        public IRepository<File> GetFileRepo()
        {
            return new FileRepo();
        }

        public IRepository<Switch> GetSwitchRepo()
        {
            return new SwitchRepo();
        }
       
    }
}
