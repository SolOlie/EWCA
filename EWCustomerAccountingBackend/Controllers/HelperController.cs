using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DAL;
using DAL.Repositories;
using Entities.Entities;

namespace EWCustomerAccountingBackend.Controllers
{
    
    public class HelperController : Controller
    {
        private IRepository<Customer> db = new Facade().GetCustomerRepo();
        // GET: Helper
        public ActionResult Index()
        {
        
            return View();
        }

        DAL.DB.CADBContext db1 = new DAL.DB.CADBContext();

        [ValidateInput(false)]
        public ActionResult CustomerTestTablePartial()
        {
            var model = db1.Customers;
            return PartialView("_CustomerTestTablePartial", model.ToList());
        }
       

        
    }
}