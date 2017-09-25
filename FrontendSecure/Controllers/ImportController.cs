using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
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

        [HttpGet]
        public FileResult Export(int customerId)
        {
            Customer c = dbc.Read(customerId);
            List<Asset> assets = db.ReadAllWithFk(customerId);


            Microsoft.Office.Interop.Excel.Application oXL;
            Microsoft.Office.Interop.Excel._Workbook oWB;
            Microsoft.Office.Interop.Excel._Worksheet oSheet;
            Microsoft.Office.Interop.Excel.Range oRng;
            object misvalue = System.Reflection.Missing.Value;
            try
            {
                //Start Excel and get Application object.
                oXL = new Microsoft.Office.Interop.Excel.Application();
                oXL.Visible = true;

                //Get a new workbook.
                oWB = (Excel._Workbook) (oXL.Workbooks.Add(""));
                oSheet = (Excel._Worksheet) oWB.ActiveSheet;

                //Add table headers going cell by cell.
                oSheet.Cells[1, 1] = "Kunde";
                oSheet.Cells[1, 2] = "Adresse";
                oSheet.Cells[1, 3] = "Dato";

                //Format A1:D1 as bold, vertical alignment = center.
                oSheet.Range["A1", "C1"].Font.Bold = true;
                oSheet.Range["A1", "C1"].VerticalAlignment =
                    Excel.XlVAlign.xlVAlignCenter;


                oSheet.Cells[2, 1] = c.Firm;
                oSheet.Cells[2, 2] = c.Address;
                oSheet.Cells[2, 3] = c.Date.ToShortDateString();
                // Create an array to multiple values at once.

                oSheet.Cells[4, 1] = "Navn";
                oSheet.Cells[4, 2] = "Beskrivelse";
                oSheet.Cells[4, 3] = "Adresse";
                oSheet.Cells[4, 4] = "Lokation";
                oSheet.Cells[4, 5] = "Login";
                oSheet.Cells[4, 6] = "Password";
                oSheet.Cells[4, 7] = "Note";
                oSheet.Cells[4, 8] = "RAM";
                oSheet.Cells[4, 9] = "HDD";
                oSheet.Cells[4, 10] = "Type";
                oSheet.Cells[4, 11] = "Bruger";
                oSheet.Cells[4, 12] = "IP";
                oSheet.Cells[4, 13] = "OS";
                oSheet.Cells[4, 14] = "Dato";
                oSheet.Range["A4", "N4"].Font.Bold = true;

                for (int row = 0; row < assets.Count; row++)
                {
                    var a = assets[row];
                    int rowtofill = row + 5;
                    oSheet.Cells[rowtofill, 1] = a.Name;
                    oSheet.Cells[rowtofill, 2] = a.Description;
                    oSheet.Cells[rowtofill, 3] = a.Address;
                    oSheet.Cells[rowtofill, 4] = a.Location;
                    oSheet.Cells[rowtofill, 5] = a.Login;
                    oSheet.Cells[rowtofill, 6] = a.Password;
                    oSheet.Cells[rowtofill, 7] = a.Note;
                    oSheet.Cells[rowtofill, 8] = a.RAM;
                    oSheet.Cells[rowtofill, 9] = a.HDD;
                    oSheet.Cells[rowtofill, 10] = a.Type.Description;
                    oSheet.Cells[rowtofill, 11] = a.Usedby;
                    oSheet.Cells[rowtofill, 12] = a.IpAddress;
                    oSheet.Cells[rowtofill, 13] = a.OS;
                    oSheet.Cells[rowtofill, 14] = a.InstallationDate.ToShortDateString();
                }


                
                oRng = oSheet.UsedRange;
                oRng.EntireColumn.AutoFit();

                oXL.Visible = false;
                oXL.UserControl = false;
                string path = Server.MapPath("~//Content//TempImports//"+c.Firm+".xls");
                oWB.SaveAs(path, Excel.XlFileFormat.xlWorkbookDefault,
                    Type.Missing, Type.Missing,
                    false, false, Excel.XlSaveAsAccessMode.xlNoChange,
                    Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
              
                oWB.Close();
                oXL.Quit();

                string filename = "Export.xlsx";
                string filepath = AppDomain.CurrentDomain.BaseDirectory + "/Content/TempImports/"+c.Firm+".xls";
                byte[] filedata = System.IO.File.ReadAllBytes(filepath);
                string contentType = MimeMapping.GetMimeMapping(filepath);

                var cd = new System.Net.Mime.ContentDisposition
                {
                    FileName = filename,
                    Inline = true,
                };

                Response.AppendHeader("Content-Disposition", cd.ToString());

                System.IO.File.Delete(path);
                return File(filedata, contentType);

               

            }
            catch (Exception ex)
            {
                throw ex;
            }
           
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