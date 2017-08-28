using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Entities.Entities;
using FrontendSecure.Gateways;
using FrontendSecure.Models;

namespace FrontendSecure.Controllers
{
    public class SearchController : Controller
    {
        private IServiceGateway<User> dbu = new BllFacade().GetUserGateway();
        private IServiceGateway<Asset> dba = new BllFacade().GetAssetGateway();
        private IServiceGateway<Customer> dbc = new BllFacade().GetCustomerGateway();
        // GET: Search
        public ActionResult Index(string searchString)
        {
            var usersList = new List<User>();
            var assetList = new List<Asset>();
            var customerList = new List<Customer>();
            

            if (Session["loggedInCustomerId"] == null)
            {
                var model1 = new SearchModel()
                {
                    Users = usersList,
                    Customers = customerList,
                    Assets = assetList
                };
                return View(model1);
            }
            int customerId = (int) Session["loggedInCustomerId"];
            if (customerId == 1)
            {
                 usersList = dbu.ReadAll();
             assetList = dba.ReadAll();
             customerList = dbc.ReadAll();
            }
            else
            {
                usersList = dbu.ReadAllWithFk(customerId);
                assetList = dba.ReadAllWithFk(customerId);
                customerList = new List<Customer>() {dbc.Read(customerId)};
            }
            
            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();
                var searchListAsset = new List<Asset>();
            searchListAsset.AddRange(
            assetList.Where(c => c.Name.ToLower().Contains(searchString) || c.Type.Description.ToLower().Contains(searchString)));

            var searchListUser = new List<User>();
            searchListUser.AddRange(
            usersList.Where(c => c.FirstName.ToLower().Contains(searchString) || c.Email.ToLower().Contains(searchString)));

            var searchListCustomer = new List<Customer>();
            searchListCustomer.AddRange(
            customerList.Where(c => c.Firm.ToLower().Contains(searchString)));

                usersList = searchListUser;
                customerList = searchListCustomer;
                assetList = searchListAsset;
            }
            var model = new SearchModel()
            {
                Users = usersList,
                Customers = customerList,
                Assets = assetList
            };
            return View(model);
        }
    }
}