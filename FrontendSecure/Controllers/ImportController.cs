using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Entities.Entities;
using FrontendSecure.Gateways;
using Excel = Microsoft.Office.Interop.Excel;

namespace FrontendSecure.Controllers
{
    public class ImportController : Controller
    {
        private IServiceGateway<Asset> db = new BllFacade().GetAssetGateway();
        private IServiceGateway<AssetType> dbaT = new BllFacade().GetAssetTypeGateway();
        private IServiceGateway<Customer> dbc = new BllFacade().GetCustomerGateway();

        // GET: Import
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Import(HttpPostedFileBase excelfile)
        {
            if (excelfile == null || excelfile.ContentLength == 0)
            {
                ViewBag.Error = "please select an excel file";
                return View("Index");
            }
            else
            {
                if (excelfile.FileName.EndsWith("xls") || excelfile.FileName.EndsWith("xlsx"))
                {
                    string path = Server.MapPath("~/Content/TempImports/" + excelfile.FileName);

                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                    }
                    excelfile.SaveAs(path);
                    //read data
                    Excel.Application application = new Excel.Application();
                    var wb = application.Workbooks;
                    Excel.Workbook workbook = wb.Open(path);
                    Excel.Worksheet worksheet = workbook.ActiveSheet;
                    Excel.Range range = worksheet.UsedRange;


                    //read customer
                    Customer customer = new Customer();

                    customer.Firm = ((Excel.Range)range.Cells[3, 1]).Text;
                    customer.Address = ((Excel.Range)range.Cells[3, 2]).Text;
                    string date = ((Excel.Range)range.Cells[3, 3]).Text;

                    if (string.IsNullOrEmpty(date?.Trim()))
                    {
                        customer.Date = DateTime.Now;
                    }
                    else
                    {
                        var datetime = DateTime.Now;
                        var isparsed = DateTime.TryParseExact(date, "d-M-yyyy", null, DateTimeStyles.AllowWhiteSpaces,
                           out datetime);
                        if (isparsed)
                        {
                            customer.Date = datetime;
                        }
                        else
                        {
                            customer.Date = DateTime.Now;
                        }

                    }
                    // read Assets + type + switch
                    List<Asset> assets = new List<Asset>();
                    List<AssetType> assetTypes = new List<AssetType>();

                    for (int row = 8; row < range.Rows.Count+1; row++)
                    {
                        Asset p = new Asset();

                        
                        p.Name = ((Excel.Range)range.Cells[row, 1]).Text;
                        p.Address = ((Excel.Range)range.Cells[row, 3]).Text;
                        p.Description = ((Excel.Range)range.Cells[row, 2]).Text;
                        p.HDD = ((Excel.Range)range.Cells[row, 9]).Text;
                        p.RAM = ((Excel.Range)range.Cells[row, 8]).Text;
                        p.IpAddress = ((Excel.Range)range.Cells[row, 12]).Text;
                        p.Location = ((Excel.Range)range.Cells[row, 4]).Text;
                        p.Login = ((Excel.Range)range.Cells[row, 5]).Text;
                        p.Usedby = ((Excel.Range)range.Cells[row, 11]).Text;
                        p.Password = ((Excel.Range)range.Cells[row, 6]).Text;
                        p.Note = ((Excel.Range)range.Cells[row, 7]).Text;
                        p.OS = ((Excel.Range)range.Cells[row, 13]).Text;
                        string type = ((Excel.Range)range.Cells[row, 10]).Text;


                        // date
                        string dateAsset = ((Excel.Range)range.Cells[row, 14]).Text;

                        if (string.IsNullOrEmpty(dateAsset?.Trim()))
                        {
                            p.InstallationDate = DateTime.Now;
                        }
                        else
                        {
                            var datetime = DateTime.Now;
                            var isparsed = DateTime.TryParseExact(dateAsset, "d-M-yyyy", null, DateTimeStyles.AllowWhiteSpaces, out datetime);
                            p.InstallationDate = isparsed ? datetime : DateTime.Now;
                        }
                        // date end

                        // type
                        if (assetTypes.Count(x => x.Description.ToLower().Equals(type.ToLower())) < 1)
                        {
                            var typeAsset= new AssetType()
                            {
                                Description = type.ToLower()
                            };
                            assetTypes.Add(typeAsset);
                            p.Type = typeAsset;
                        }
                        else
                        {
                            p.Type= assetTypes.FirstOrDefault(x => x.Description.ToLower().Equals(type.ToLower()));
                        }
                        // type end

                        assets.Add(p);

                    }
                    // Add to Database

                    var alreadyaddededtypes = dbaT.ReadAll();
                    int i = 0;
                    foreach (var t in assetTypes)
                    {
                        foreach (var at in alreadyaddededtypes)
                        {
                            if (t.Description.ToLower().Equals(at.Description.ToLower()))
                            {
                                i++;
                                break;
                            }
                        }
                        if (i==0)
                        {
                            dbaT.Create(t);
                        }
                        i = 0;
                    }
                    alreadyaddededtypes = dbaT.ReadAll();

                    var cus = dbc.Create(customer);

                    foreach (var a in assets)
                    {
                        a.CustomerId = cus.Id;
                        a.Customer = cus;
                        a.Type =
                            alreadyaddededtypes.FirstOrDefault(x => x.Description.ToLower().Equals(a.Type.Description.ToLower()));
                        a.TypeId = a.Type?.Id;
                        db.Create(a);
                    }
                   
                    workbook.Close(false);
                    wb.Close();
                    application.Quit();
                    
                    System.IO.File.Delete(path);
                    return RedirectToAction("Index", "Customers");
                }
                else
                {
                    ViewBag.Error = "file type is incorrect";
                    return View("Index");
                }
            }
        }
    }
}