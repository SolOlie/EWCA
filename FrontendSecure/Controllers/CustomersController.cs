﻿using System;
using DevExpress.Web.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using DevExpress.Web;
using Entities.Entities;
using FrontendSecure.Gateways;
using FrontendSecure.Models;
using WebGrease;

namespace FrontendSecure.Controllers
{
    [Authorize]
    public class CustomersController : Controller
    {
        private readonly IServiceGateway<Customer> db = new BllFacade().GetCustomerGateway();
        private readonly IServiceGateway<Asset> dbAsset = new BllFacade().GetAssetGateway();
        private readonly IServiceGateway<AssetType> dbAssetType = new BllFacade().GetAssetTypeGateway();
        private readonly IServiceGateway<Manufacturer> dbManufacturer = new BllFacade().GetManufacturerGateway();
        private readonly IServiceGateway<Port> dbPort = new BllFacade().GetPortGateway();
        private readonly IServiceGateway<User> dbUser = new BllFacade().GetUserGateway();
        private readonly IServiceGateway<File> dbFile = new BllFacade().GetFileGateway();
        private static List<Port> ports;

        private enum AuthState
        {
            NoAuth,
            UserAuth,
            AdminAuth,
            ElitewebAuth
        };
        private AuthState isAuthorized(int customerId)
        {
            // return AuthState.ElitewebAuth;
            HttpCookie myCookie = Request.Cookies["UserCookie"];
            if (myCookie == null)
            {
                return AuthState.NoAuth;
            }
            int i = 0;
            //ok - cookie is found.
            //Gracefully check if the cookie has the key-value as expected.
            if (!string.IsNullOrEmpty(myCookie.Values["userid"]))
            {
                string userId = myCookie.Values["userid"].ToString();
                int.TryParse(userId, out i);

                //Yes userId is found. Mission accomplished.
            }




            int loggedinUserId = i;
            var loggedInUser = dbUser.Read(loggedinUserId);

            if (loggedInUser == null)
            {
                return AuthState.NoAuth;
            }

            if (loggedInUser.IsContactForCustomer.Id > 0)
            {
                if (1 == loggedInUser.IsContactForCustomer.Id)
                {
                    return AuthState.ElitewebAuth;
                }
                if (customerId == loggedInUser.IsContactForCustomer.Id)
                {
                    if (loggedInUser.IsAdmin)
                    {
                        return AuthState.AdminAuth;
                    }
                    return AuthState.UserAuth;
                }

                return AuthState.NoAuth;

            }
            return AuthState.NoAuth;
        }
        // GET: Customers
        public ActionResult Index()
        {
            if (isAuthorized(1) != AuthState.ElitewebAuth) //Id 1 er id for EliteWeb
            {
                return View("NotAuthorized");
            }
            var searchList = new List<Customer>();

            return View(searchList);
        }

        // GET: Customers/Details/5
        public ActionResult Details(int? id, string activeTab)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            switch (isAuthorized(id.Value))
            {
                case AuthState.NoAuth:
                    return View("NotAuthorized");
                default:
                    var s = db.Read(id.Value);
                    var model = new CustomerAssetypeViewModel()
                    {
                        Customer = s,
                    };

                    if (model.Customer == null)
                    {
                        return HttpNotFound();
                    }
                    s = null;
                    return View(model);

            }

        }

        public ActionResult SortedAssets(int customerId, int typeId)
        {

            Customer c = db.Read(customerId);
            if (c == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var model = new CustomerAssetypeViewModel()
            {
                Customer = c,
                SortedAssets = dbAsset.ReadAllWithFk(customerId).FindAll(x => x.Type.Id == typeId)
            };

            return View(model);
        }
        public ActionResult AssetDetails(int? id, int customerId)
        {
            switch (isAuthorized(customerId))
            {
                case AuthState.NoAuth:
                    return View("NotAuthorized");
                default:
                    if (id == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                    Asset asset = dbAsset.Read(id.Value);

                    if (asset == null)
                    {
                        return HttpNotFound();
                    }
                    var model = new CreateChangelogModel()
                    {
                        Asset = asset,
                    };
                    return View(model);

            }

        }

        // GET: Customers/Create
        public ActionResult Create()
        {
            if (isAuthorized(1) != AuthState.ElitewebAuth) //Id 1 er id for EliteWeb
            {
                return View("NotAuthorized");
            }
            var model = new CreateCustomerModel()
            {
                Users = dbUser.ReadAll(),
                Customer = new Customer()
                {
                    Date = DateTime.Now
                }
            };

            return View(model);
        }
        public ActionResult CreateAsset(int id)
        {
            switch (isAuthorized(id))
            {
                case AuthState.NoAuth:
                    return View("NotAuthorized");
                default:
                    var model = new CreateAssetModel()
                    {
                        customerassetlist = dbAsset.ReadAllWithFk(id),
                        Users = dbUser.ReadAllWithFk(id),
                        customerId = id,
                        Customers = new List<Customer>() { db.Read(id) },
                        Manufacturers = dbManufacturer.ReadAll(),
                        AssetTypes = dbAssetType.ReadAll(),
                        Asset = new Asset()
                        {
                            InstallationDate = DateTime.Now
                        }
                    };
                    ports = new List<Port>();

                    return View(model);

            }

        }

        // POST: Customers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int[] ids, Customer customer)
        {
            if (isAuthorized(1) != AuthState.ElitewebAuth) //Id 1 er id for EliteWeb
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
        public ActionResult CreateAsset(Asset asset, HttpPostedFileBase upload, string atInput, string atInputManu)
        {
            switch (isAuthorized(asset.Customer.Id))
            {
                case AuthState.NoAuth:
                    return View("NotAuthorized");
                default:
                    if (ModelState.IsValid)
                    {
                        if (!string.IsNullOrEmpty(atInput) )
                        {
                            var type = dbAssetType.Create(new AssetType
                            {
                                Description = atInput

                            });
                          
                            asset.Type = type;
                            asset.TypeId = type.Id;
                        }
                        else
                        {
                           
                            asset.TypeId = asset.Type.Id;
                            asset.Type = dbAssetType.Read(asset.Type.Id);
                        }
                        if (!string.IsNullOrEmpty(atInputManu))
                        {
                            var manu = dbManufacturer.Create(new Manufacturer
                            {
                                manufacturer = atInputManu
                            });
                            asset.Manufacturer = manu;
                            asset.ManufacturerId = manu.Id;
                        }
                        else
                        {
                            asset.Manufacturer = dbManufacturer.Read(asset.Manufacturer.Id);
                            asset.ManufacturerId = asset.Manufacturer.Id;
                        }

                        var sw = new Switch();

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
                            asset.FileAttachments = new List<File>() { attachment };
                        }

                        var a = dbAsset.Create(asset);

                        var aa = dbAsset.Read(a.Id);

                        foreach (var port in ports)
                        {
                            port.SwitchId = aa.Switch.Id;
                            port.Switch = null;
                            port.Asset = null;
                            dbPort.Create(port);
                        }

                        ports = new List<Port>();

                        return RedirectToAction("Details", new { id = asset.Customer.Id });
                    }


                    //FAILED
                    var model = new CreateAssetModel()
                    {
                        Users = dbUser.ReadAll(),
                        Customers = new List<Customer>() { db.Read(asset.Customer.Id) },
                        AssetTypes = dbAssetType.ReadAll(),
                        Manufacturers = dbManufacturer.ReadAll(),
                        Asset = asset
                    };


                    return View(model);

            }

        }

        // GET: Customers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (isAuthorized(1) != AuthState.ElitewebAuth) //Id 1 er id for EliteWeb
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
            if (isAuthorized(1) != AuthState.ElitewebAuth) //Id 1 er id for EliteWeb
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

        [HttpGet]
        public FileResult Download(int Id)
        {

            var file = dbFile.Read(Id);
            return File(file.ContentFile.Content, file.ContentType, file.Name);
        }


        /// <summary>
        /// partialmethods
        /// </summary>
        /// <returns></returns>

        [ValidateInput(false)]
        public ActionResult CustomerTableExpressPartial()
        {
            if (isAuthorized(1) != AuthState.ElitewebAuth) //Id 1 er id for EliteWeb
            {
                return View("NotAuthorized");
            }
            var model = db.ReadAll();
            return PartialView("_CustomerTableExpressPartial", model);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult CustomerTableExpressPartialDelete(string Id)
        {
            if (isAuthorized(1) != AuthState.ElitewebAuth) //Id 1 er id for EliteWeb/2do
            {
                return View("NotAuthorized");
            }
            Id = Id.Replace("\"", "");
            if (string.IsNullOrEmpty(Id))
            {
                Id = "0";
            }
            int id = Int32.Parse(Id);
            var model = new List<Customer>();
            if (id > 1)
            {
                try
                {
                    var c = new Customer()
                    {
                        Id = id
                    };
                    db.Delete(c);

                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            if (id == 1)
            {
                try
                {
                    throw new Exception("Cannot delete");
                }
                catch (Exception e)
                {

                    ViewData["EditError"] = e.Message;
                }

            }
            model = db.ReadAll();
            return PartialView("_CustomerTableExpressPartial", model);
        }

        public bool AddToPortListAjax(int portnumber, string asset, string trunk, string VLAN, string note)
        {
            var p = new Port()
            {
                PortNumber = portnumber,
                Note = note,
                Trunk = trunk,
                VLAN = VLAN,
                Asset = asset
            };
            ports.Add(p);
            return true;
        }

        public ActionResult RenderPortPartial()
        {
            return PartialView("PortListPartialView", ports);
        }



        
    }
}
