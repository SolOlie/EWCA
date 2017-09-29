﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using Entities.Entities;
using FrontendSecure.Gateways;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Excel = Microsoft.Office.Interop.Excel;

namespace FrontendSecure.Controllers
{
    [Authorize]
    public class ImportController : Controller
    {
        private IServiceGateway<Asset> db = new BllFacade().GetAssetGateway();
        private IServiceGateway<AssetType> dbaT = new BllFacade().GetAssetTypeGateway();
        private IServiceGateway<Customer> dbc = new BllFacade().GetCustomerGateway();
        private IServiceGateway<User> dbu = new BllFacade().GetUserGateway();
        private enum AuthStates
        {
            NoAuth,
            UserAuth,
            AdminAuth,
            ElitewebAuth
        };
        private AuthStates isAuthorized(int customerId)
        {
            //return AuthStates.ElitewebAuth;
            var session = Session["loggedinUserId"];
            if (session == null)
            {
                return AuthStates.NoAuth;
            }

            int loggedinUserId = (int)session;
            var loggedInUser = dbu.Read(loggedinUserId);

            if (loggedInUser == null)
            {
                return AuthStates.NoAuth;
            }

            if (loggedInUser.IsContactForCustomer.Id > 0)
            {
                if (1 == loggedInUser.IsContactForCustomer.Id)
                {
                    return AuthStates.ElitewebAuth;
                }
                if (customerId == loggedInUser.IsContactForCustomer.Id)
                {
                    if (loggedInUser.IsAdmin)
                    {
                        return AuthStates.AdminAuth;
                    }
                    return AuthStates.UserAuth;
                }

                return AuthStates.NoAuth;

            }
            return AuthStates.NoAuth;
        }

        // GET: Import
        public ActionResult Index()
        {
            if (isAuthorized(1) != AuthStates.ElitewebAuth)
            {
                return View("NotAuthorized");
            }
            return View();
        }


        [HttpGet]
        public void Export(int customerId)
        {
            if (isAuthorized(1) != AuthStates.ElitewebAuth)
            {
                return;
            }
            Customer c = dbc.Read(customerId);
            List<Asset> assets = db.ReadAllWithFk(customerId);

            HSSFWorkbook hssfwb = new HSSFWorkbook();
            ICellStyle style = hssfwb.CreateCellStyle();
            style.BorderBottom = BorderStyle.Medium;
            style.BottomBorderColor = HSSFColor.Red.Index;
            IFont font = hssfwb.CreateFont();
            font.IsBold = true;
            style.SetFont(font);
            
            ISheet sheet = hssfwb.CreateSheet("Export");
            IRow row1 = sheet.CreateRow(0);
            row1.CreateCell(0).SetCellValue("Kunde");
            row1.CreateCell(1).SetCellValue("Adresse");
            row1.CreateCell(2).SetCellValue("Dato");
            for (int i = 0; i < 3; i++)
            {
                row1.GetCell(i).CellStyle = style;
            }
            row1 = sheet.CreateRow(1);
            row1.CreateCell(0).SetCellValue(c.Firm);
            row1.CreateCell(1).SetCellValue(c.Address);
            row1.CreateCell(2).SetCellValue(c.Date.ToShortDateString());

            try
            {
                // Create an array to multiple values at once.
                row1 = sheet.CreateRow(3);
                row1.CreateCell(0).SetCellValue("Navn");
                row1.CreateCell(1).SetCellValue("Beskrivelse");
                row1.CreateCell(2).SetCellValue("Adresse");
                row1.CreateCell(3).SetCellValue("Lokation");
                row1.CreateCell(4).SetCellValue("Login");
                row1.CreateCell(5).SetCellValue("Password");
                row1.CreateCell(6).SetCellValue("Note");
                row1.CreateCell(7).SetCellValue("RAM");
                row1.CreateCell(8).SetCellValue("HDD");
                row1.CreateCell(9).SetCellValue("Type");
                row1.CreateCell(10).SetCellValue("Bruger");
                row1.CreateCell(11).SetCellValue("IP");
                row1.CreateCell(12).SetCellValue("OS");
                row1.CreateCell(13).SetCellValue("Dato");
                for (int i = 0; i < 14; i++)
                {
                    row1.GetCell(i).CellStyle = style;
                }

                for (int row = 0; row < assets.Count; row++)
                {
                    var a = assets[row];
                    int rowtofill = row + 4;
                    row1 = sheet.CreateRow(rowtofill);
                    row1.CreateCell(0).SetCellValue(a.Name);
                    row1.CreateCell(1).SetCellValue(a.Description);
                    row1.CreateCell(2).SetCellValue(a.Address);
                    row1.CreateCell(3).SetCellValue(a.Location);
                    row1.CreateCell(4).SetCellValue(a.Login);
                    row1.CreateCell(5).SetCellValue(a.Password);
                    row1.CreateCell(6).SetCellValue(a.Note);
                    row1.CreateCell(7).SetCellValue(a.RAM);
                    row1.CreateCell(8).SetCellValue(a.HDD);
                    row1.CreateCell(9).SetCellValue(a.Type.Description);
                    row1.CreateCell(10).SetCellValue(a.Usedby);
                    row1.CreateCell(11).SetCellValue(a.IpAddress);
                    row1.CreateCell(12).SetCellValue(a.OS);
                    row1.CreateCell(13).SetCellValue(a.InstallationDate.ToShortDateString());

                }
                for (int i = 0; i <= 13; i++)
                {
                    sheet.AutoSizeColumn(i);
                }
                string filename = "Export.xls";
                var cd = new System.Net.Mime.ContentDisposition
                {
                    FileName = filename,
                    Inline = true,
                };
                Response.ContentType = "application/vnd.ms-excel";
                Response.AddHeader("Content-Disposition", cd.ToString());
                Response.Clear();
                MemoryStream file = new MemoryStream();
                hssfwb.Write(file);
                Response.BinaryWrite(file.GetBuffer());
                Response.End();


            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        private void readfile(ISheet sheet, string path)
        {
            //read customer
            Customer customer = new Customer();

            customer.Firm = sheet.GetRow(2).GetCell(0).StringCellValue;
            customer.Address = sheet.GetRow(2).GetCell(1).StringCellValue;
            if (sheet.GetRow(2).GetCell(2) == null)
            {
                customer.Date = DateTime.Now;
            }
            else
            {
                customer.Date = sheet.GetRow(2).GetCell(2).DateCellValue;
            }


            //if (string.IsNullOrEmpty(date?.Trim()))
            //{
            //    customer.Date = DateTime.Now;
            //}
            //else
            //{
            //    var datetime = DateTime.Now;
            //    var isparsed = DateTime.TryParseExact(date, "d-M-yyyy", null, DateTimeStyles.AllowWhiteSpaces,
            //       out datetime);
            //    if (isparsed)
            //    {
            //        customer.Date = datetime;
            //    }
            //    else
            //    {
            //        customer.Date = DateTime.Now;
            //    }

            //}
            // read Assets + type + switch
            List<Asset> assets = new List<Asset>();
            List<AssetType> assetTypes = new List<AssetType>();

            for (int row = 7; row < sheet.LastRowNum + 1; row++)
            {
                Asset p = new Asset();


                p.Name = sheet.GetRow(row).GetCell(0).StringCellValue;
                p.Address = sheet.GetRow(row).GetCell(2).StringCellValue;
                p.Description = sheet.GetRow(row).GetCell(1).StringCellValue;
                p.HDD = sheet.GetRow(row).GetCell(8).StringCellValue;
                p.RAM = sheet.GetRow(row).GetCell(7).StringCellValue;
                p.IpAddress = sheet.GetRow(row).GetCell(11).StringCellValue;
                p.Location = sheet.GetRow(row).GetCell(3).StringCellValue;
                p.Login = sheet.GetRow(row).GetCell(4).StringCellValue;
                p.Usedby = sheet.GetRow(row).GetCell(10).StringCellValue;
                p.Password = sheet.GetRow(row).GetCell(5).StringCellValue;
                p.Note = sheet.GetRow(row).GetCell(6).StringCellValue;
                p.OS = sheet.GetRow(row).GetCell(12).StringCellValue;
                string type = sheet.GetRow(row).GetCell(9).StringCellValue;


                // date
                if (sheet.GetRow(row).GetCell(13) == null)
                {
                    p.InstallationDate = DateTime.Now;
                }
                else
                {
                    p.InstallationDate = sheet.GetRow(row).GetCell(13).DateCellValue;
                    if (p.InstallationDate.Year == 1)
                    {
                        p.InstallationDate = DateTime.Now;
                    }
                }
                
                //string dateAsset = sheet.GetRow(row).GetCell(13).StringCellValue;

                //if (string.IsNullOrEmpty(dateAsset?.Trim()))
                //{
                //    p.InstallationDate = DateTime.Now;
                //}
                //else
                //{
                //    var datetime = DateTime.Now;
                //    var isparsed = DateTime.TryParseExact(dateAsset, "d-M-yyyy", null, DateTimeStyles.AllowWhiteSpaces, out datetime);
                //    p.InstallationDate = isparsed ? datetime : DateTime.Now;
                //}
                // date end

                // type
                if (assetTypes.Count(x => x.Description.ToLower().Equals(type.ToLower())) < 1)
                {
                    var typeAsset = new AssetType()
                    {
                        Description = type.ToLower()
                    };
                    assetTypes.Add(typeAsset);
                    p.Type = typeAsset;
                }
                else
                {
                    p.Type = assetTypes.FirstOrDefault(x => x.Description.ToLower().Equals(type.ToLower()));
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
                if (i == 0)
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
            System.IO.File.Delete(path);
        }

        private string saveFile(HttpPostedFileBase fileBase)
        {
            string path = Server.MapPath("~/Content/TempImports/" + fileBase.FileName);

            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
            fileBase.SaveAs(path);
            return path;
        }
        [HttpPost]
        public ActionResult Import(HttpPostedFileBase excelfile)
        {
            if (isAuthorized(1) != AuthStates.ElitewebAuth)
            {
                return View("NotAuthorized");
            }
            if (excelfile == null || excelfile.ContentLength == 0)
            {
                ViewBag.Error = "please select an excel file";
                return View("Index");
            }
            else
            {
                ISheet sheet;
                if (excelfile.FileName.EndsWith("xls"))
                {
                    var path = saveFile(excelfile);
                    HSSFWorkbook hssfwb = new HSSFWorkbook(excelfile.InputStream);
                    sheet = hssfwb.GetSheetAt(0);
                    readfile(sheet, path);
                    //read data

                    return RedirectToAction("Index", "Customers");
                }
                else if (excelfile.FileName.EndsWith("xlsx"))
                {
                    var path = saveFile(excelfile);
                    XSSFWorkbook hssfwb = new XSSFWorkbook(excelfile.InputStream);
                    sheet = hssfwb.GetSheetAt(0);
                    readfile(sheet, path);
                    //read data

                    return RedirectToAction("Index", "Customers");
                }

                else
                {
                    ViewBag.Error = "file type is incorrect";
                    return View("Index");
                }
            }
        }

        public FileResult DownloadTemplate()
        {
            if (isAuthorized(1) != AuthStates.ElitewebAuth)
            {
                return null;
            }
            string filename = "Template.xlsx";
            string filepath = AppDomain.CurrentDomain.BaseDirectory + "/Content/Template.xlsx";
            byte[] filedata = System.IO.File.ReadAllBytes(filepath);
            string contentType = MimeMapping.GetMimeMapping(filepath);

            var cd = new System.Net.Mime.ContentDisposition
            {
                FileName = filename,
                Inline = true,
            };

            Response.AppendHeader("Content-Disposition", cd.ToString());


            return File(filedata, contentType);

        }
    }
}