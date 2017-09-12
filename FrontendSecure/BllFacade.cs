using Entities.Entities;
using FrontendSecure.Gateways;
using FrontendSecure.Gateways.SecureGateways;

namespace FrontendSecure
{
    public class BllFacade
    {
        public IServiceGateway<AssetType> GetAssetTypeGateway()
        {
            return new AssetTypeGatewaySecure();
        }
        public IServiceGateway<Asset> GetAssetGateway()
        {
            return new AssetGatewaySecure();
        }
        public IServiceGateway<Customer> GetCustomerGateway()
        {
            return new CustomerGatewaySecure();
        }
        public IServiceGateway<User> GetUserGateway()
        {
            return new UserGatewaySecure();
        }
        public IServiceGateway<Changelog> GetChangelogGateway()
        {
            return new ChangelogGatewaySecure();
        }

        public IServiceGateway<File> GetFileGateway()
        {
            return new FileGatewaySecure();
        }

        public IServiceGateway<Switch> GetSwitchGateway()
        {
            return new SwitchGatewaySecure();
        }

        public IServiceGateway<Port> GetPortGateway()
        {
            return new PortGatewaySecure();
        }

       }
}
