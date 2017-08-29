using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Entities.Entities;
using FrontendSecure.Gateways;
using FrontendSecure.Models;
using WebGrease;

namespace FrontendSecure.Controllers
{
    //[Authorize] //det virker nu
    public class CustomersController : Controller
    {
        private readonly IServiceGateway<Customer> db = new BllFacade().GetCustomerGateway();

        private readonly IServiceGateway<Asset> dbAsset = new BllFacade().GetAssetGateway();

        private readonly IServiceGateway<AssetType> dbAssetType = new BllFacade().GetAssetTypeGateway();


        private readonly IServiceGateway<User> dbUser = new BllFacade().GetUserGateway();
        private readonly IServiceGateway<File> dbFile = new BllFacade().GetFileGateway();
       
        private bool isAuthorized(int customerId)
        {
            return true;
            var session = Session["loggedInCustomerId"];
            if (session == null)
            {
                return false;
            }

            int loggedInCustomerId = (int)session;

            
            if (loggedInCustomerId > 0)
            {
                if (customerId == loggedInCustomerId || loggedInCustomerId == 1)
                {
                    return true;
                }

            }
            return false;
        }
        // GET: Customers
        public ActionResult Index(string searchString)
        {
            if (!isAuthorized(1)) //Id 1 er id for EliteWeb
            {
                //return RedirectToAction("Index", "Home");
            }
            var searchList = new List<Customer>();
            if (!string.IsNullOrEmpty(searchString))
            {
                var clist = db.ReadAll();
           
            foreach (var c in clist)
            {
                if (c.Firm.Contains(searchString))
                {
                    searchList.Add(c);
                }
            }
            }
            else
            {
                searchList = db.ReadAll();
            }
            
            return View(searchList);
        }

        // GET: Customers/Details/5
        public ActionResult Details(int? id, string searchString, string activeTab)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (!isAuthorized(id.Value))
            {

                return View("NotAuthorized");
            }
            var s = db.Read(id.Value);
            var userList = s.ContactPersons;
            var searchList = dbAsset.ReadAllWithFk(id.Value);
            if (!string.IsNullOrEmpty(searchString))
            {
                if (activeTab != null)
                {
                    if (activeTab.Equals("AssetPane"))
                    {
                        var clist = searchList;
                        searchList = new List<Asset>();
                        searchList.AddRange(
                            clist.Where(c => c.Name.ToLower().Contains(searchString.ToLower()) || c.Type.Description.ToLower().Contains(searchString.ToLower())));
                    }
                    else if (activeTab.Equals("UserPane"))
                    {
                        var clist = userList;
                        userList = new List<User>();
                        userList.AddRange(
                            clist.Where(
                                c =>
                                    c.FirstName.ToLower().Contains(searchString.ToLower()) || c.Email.ToLower().Contains(searchString.ToLower())));

                    }
                }
            }

            var model = new CustomerAssetypeViewModel()
            {
                Users = userList,
                Customer = s,
                SortedAssets = searchList,
                AssetTypes = dbAssetType.ReadAll()
        };
           
            if (model.Customer == null || model.AssetTypes == null)
            {
                return HttpNotFound();
            }
          
            return View(model);
        }

        public ActionResult SortedAssets(int customerId, int typeId)
        {
            if (!isAuthorized(customerId))
            {
                return View("NotAuthorized");
            }
            Customer c = db.Read(customerId);
            if (c == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var model = new CustomerAssetypeViewModel()
            {
                Customer = c,
                SortedAssets = dbAsset.ReadAllWithFk(customerId).FindAll(x=>x.Type.Id == typeId)
            };

            return View(model);
        }
        public ActionResult AssetDetails(int? id, int customerId)
        {
            if (!isAuthorized(customerId))
            {
                return View("NotAuthorized");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Asset asset = dbAsset.Read(id.Value);

            if (asset == null)
            {
                return HttpNotFound();
            }
            var users = dbUser.ReadAllWithFk(customerId);
            var model = new CreateChangelogModel()
            {
                Asset = asset,
                Users = users,
                
            }
            ;

            return View(model);
        }

        // GET: Customers/Create
        public ActionResult Create()
        {
            if (!isAuthorized(1))
            {
                return View("NotAuthorized");
            }
            var model = new CreateCustomerModel()
            {
                 Users = dbUser.ReadAll(),
                 Customer = new Customer()
            };
           
            return View(model);
        }
        public ActionResult CreateAsset(int id)
        {
            if (!isAuthorized(id))
            {
                return View("NotAuthorized");
            }
            var model = new CreateAssetModel()
            {
                Users = dbUser.ReadAllWithFk(id),
                customerId = id,
                Customers = new List<Customer>() {db.Read(id)},
                AssetTypes = dbAssetType.ReadAll(),
                Asset = new Asset()
            };

            return View(model);
        }
      
        // POST: Customers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int[] ids, Customer customer)
        {
            if (!isAuthorized(customer.Id))
            {
                return View("NotAuthorized");
            }
            if (ModelState.IsValid)
            {
                db.Create(customer);
                return RedirectToAction("Index");
            }
            var model = new CreateCustomerModel()
            {
                Users = dbUser.ReadAll(),
                Customer = customer
            };


            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateAsset(Asset asset, HttpPostedFileBase upload, string atInput)
        {
            if (!isAuthorized(asset.Customer.Id))
            {
                return View("NotAuthorized");
            }
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(atInput))
                {
                    asset.Type = new AssetType
                    {
                        Description = atInput
                    };
                }
                if (upload != null && upload.ContentLength > 0)
                {
                    var attachment = new File()
                    {
                        ContentType = upload.ContentType,
                        ContentFile = new ContentFile(),
                        Name = System.IO.Path.GetFileName(upload.FileName),
                    };
                    
                    using (var reader = new System.IO.BinaryReader(upload.InputStream))
                    {
                        attachment.ContentFile.Content = reader.ReadBytes(upload.ContentLength);
                    }
                    asset.FileAttachments = new List<File> (){ attachment };
                }
                dbAsset.Create(asset);
                return RedirectToAction("Details", new {id = asset.Customer.Id});
            }
            var model = new CreateAssetModel()
            {
                Users = dbUser.ReadAll(),
                Customers = db.ReadAll(),
                AssetTypes = dbAssetType.ReadAll(),
                Asset = asset
            };


            return View(model);
        }

        // GET: Customers/Edit/5
        public ActionResult Edit(int? id)
        {
           
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (!isAuthorized(id.Value))
            {
                return View("NotAuthorized");
            }
            Customer customer = db.Read(id.Value);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Firm,Address,Date")] Customer customer)
        {
            if (!isAuthorized(customer.Id))
            {
                return View("NotAuthorized");
            }
            if (ModelState.IsValid)
            {
                db.Update(customer);
                return RedirectToAction("Index");
            }
            return View(customer);
        }

        // GET: Customers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (!isAuthorized(id.Value))
            {
                return View("NotAuthorized");
            }
            Customer customer = db.Read(id.Value);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (!isAuthorized(id))
            {
                return View("NotAuthorized");
            }
            Customer customer = db.Read(id);
            db.Delete(customer);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public FileResult Download(int Id)
        {
            var file = dbFile.Read(Id);
            return File(file.ContentFile.Content,file.ContentType, file.Name);
        }

      
    }
}
