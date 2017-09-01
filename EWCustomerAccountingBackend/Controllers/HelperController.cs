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
            var list = db.ReadAll();
            return View(list);
        }

        DAL.DB.CADBContext db1 = new DAL.DB.CADBContext();

        [ValidateInput(false)]
        public ActionResult CustomerTestTablePartial()
        {
            var model = db1.Customers;
            return PartialView("_CustomerTestTablePartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult CustomerTestTablePartialAddNew([ModelBinder(typeof(DevExpressEditorsBinder))] Entities.Entities.Customer item)
        {
            var model = db1.Customers;
            if (ModelState.IsValid)
            {
                try
                {
                    model.Add(item);
                    db1.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            return PartialView("_CustomerTestTablePartial", model.ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult CustomerTestTablePartialUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] Entities.Entities.Customer item)
        {
            var model = db1.Customers;
            var modelItem = model.Include(x => x.ContactPersons).Include(x => x.Assets).FirstOrDefault(it => it.Id == item.Id);

            if (modelItem != null)
            {
                item.Assets = modelItem.Assets;
                item.ContactPersons = modelItem.ContactPersons;

            }
            if (ModelState.IsValid)
            {
                try
                {
                    
                        
                        this.UpdateModel(item);
                        db1.SaveChanges();
                    
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            return PartialView("_CustomerTestTablePartial", model.ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult CustomerTestTablePartialDelete(System.Int32 Id)
        {
            var model = db1.Customers;
            if (Id >= 0)
            {
                try
                {
                    var item = model.FirstOrDefault(it => it.Id == Id);
                    if (item != null)
                        model.Remove(item);
                    db1.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            return PartialView("_CustomerTestTablePartial", model.ToList());
        }
        /// <summary>
        /// withoutdb
        /// </summary>
        /// <returns></returns>

        [ValidateInput(false)]
        public ActionResult tabletestwithoutDBPartial()
        {
            var model = db.ReadAll();
            return PartialView("_tabletestwithoutDBPartial", model);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult tabletestwithoutDBPartialAddNew([ModelBinder(typeof(DevExpressEditorsBinder))] Entities.Entities.Customer item)
        {
            var model = new object[0];
            if (ModelState.IsValid)
            {
                try
                {
                    // Insert here a code to insert the new item in your model
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            return PartialView("_tabletestwithoutDBPartial", model);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult tabletestwithoutDBPartialUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] Entities.Entities.Customer item)
        {
            var model = new object[0];
            if (ModelState.IsValid)
            {
                try
                {
                    
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            return PartialView("_tabletestwithoutDBPartial", model);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult tabletestwithoutDBPartialDelete(System.Int32 Id)
        {
            var model = new object[0];
            if (Id >= 0)
            {
                try
                {

                    db.Delete(new Customer()
                    {
                        Id = Id
                    });
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            return PartialView("_tabletestwithoutDBPartial", model);
        }
    }
}