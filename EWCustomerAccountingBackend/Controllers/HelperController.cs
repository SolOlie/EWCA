using System;
using System.Collections.Generic;
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
            var list = db.ReadAll();
            return View(list);
        }
    }
}