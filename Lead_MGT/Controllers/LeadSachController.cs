using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq;
using System.IO;
using System.Data;
using System.Data.OleDb;
using System.Configuration;
using Lead_MGTValueObject.ImportLeadModel;
using Lead_MGTBusiness.ImportLead;
using Lead_MGTValueObject.Common;
using Lead_MGTModel.EntityModel;
using Microsoft.AspNetCore.Mvc;
using System.Web.UI.WebControls;
using System.Web.UI;
using Syncfusion.XlsIO;
using Lead_MGTValueObject.LeadSachModel;
using Lead_MGTBusiness.LeadSach;
using log4net.Config;
using log4net;

namespace Lead_MGT.Controllers
{
    public class LeadSachController : Controller
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(LogTest));
        ImportLeadBuiness obj = new ImportLeadBuiness();
        LeadSachBuiness objLeadSach = new LeadSachBuiness();
        DataTableToList objCommom = new DataTableToList();
        DatatableLeadImport objData = new DatatableLeadImport();
        List<ImportLeadModel> lis = new List<ImportLeadModel>();
        List<Lead_Import> lead =new List<Lead_Import>();
        public ActionResult Index()
        {
            List<ListLeadSachModel> ks = new List<ListLeadSachModel>();
            LeadSachFillterModel a = new LeadSachFillterModel();
            ks = objLeadSach.Index(a);
            XmlConfigurator.Configure();
            log.Info("Info message");
            return View(ks);
        }

        [HttpPost]
        public PartialViewResult Search (LeadSachFillterModel a)
        {
            var ks = objLeadSach.Search(a);
           // DataTable dt = new DataTable();
           // dt = objCommom.ToDataTable(ks);
            return PartialView(ks);
        }
        [HttpPost]
        public JsonResult LoadData(LeadSachFillterModel searchLead)
        {
            List<ListLeadSachModel> ks = new List<ListLeadSachModel>();
            LeadSachFillterModel a = new LeadSachFillterModel();
            ks = objLeadSach.Index(searchLead);
            var model = ks.Skip((searchLead.page - 1) * searchLead.pageSize).Take(searchLead.pageSize);
            int totalRow = ks.Count();
            return Json( new{
                data = model,
                total=totalRow,
                status=true
            },JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Export(LeadSachFillterModel a)
        {
            log.Info("Export");
            var ks = objLeadSach.Search(a);
            string FileName = "duongpdezai";
            var gv = new GridView();
            gv.DataSource = ks;
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;  filename=" + FileName + ";");
            Response.ContentType = "application/ms-excel";
            Response.ContentEncoding = new System.Text.UTF8Encoding();
            Response.Charset = "";
            StringWriter objStringWriter = new StringWriter();
            HtmlTextWriter objHtmlTextWriter = new HtmlTextWriter(objStringWriter);
            gv.RenderControl(objHtmlTextWriter);
            Response.Output.Write(objStringWriter.ToString());
            Response.Flush();
            Response.End();
            return null;
        }
        [HttpPost]
        public ActionResult ExportExcel(LeadSachFillterModel a)
        {
            log.Info("ExportExcel");
            var random = new Random();
            int randomnumber = random.Next();
            var ks = objLeadSach.Search(a);
            var fileName = "export_LeadSach" + randomnumber + ".xlsx";

            //Save the file to server temp folder
            string fullPath = "";
            log.Info("Createfile");
            try
            {
                 fullPath = Path.Combine(Server.MapPath("~/temp"), fileName);
            }
            catch (Exception ex)
            {
                log.Info(ex);
                throw;
            }
            
            log.Info(fullPath);
            log.Info("Createfile done");
            //Create an instance of ExcelEngine
            try
            {
                using (ExcelEngine excelEngine = new ExcelEngine())
                {
                    //Set the default application version as Excel 2016
                    excelEngine.Excel.DefaultVersion = ExcelVersion.Excel2016;

                    //Create a workbook with a worksheet
                    IWorkbook workbook = excelEngine.Excel.Workbooks.Create(1);

                    //Access first worksheet from the workbook instance
                    IWorksheet worksheet = workbook.Worksheets[0];

                    //Insert sample text into cell “A1”
                    worksheet.Range["A1"].Text = "PhoneNumber1";
                    worksheet.Range["B1"].Text = "Province";
                    worksheet.Range["C1"].Text = "BoundCode";
                    worksheet.Range["D1"].Text = "PartnerCode";
                    worksheet.Range["E1"].Text = "BandCc";
                    worksheet.Range["F1"].Text = "BandUpl";
                    worksheet.Range["G1"].Text = "CcLimit";
                    worksheet.Range["H1"].Text = "UplLimit";
                    worksheet.Range["I1"].Text = "UplInterest";
                    worksheet.Range["J1"].Text = "ValidDate";
                    worksheet.Range["K1"].Text = "PostDate";
                    worksheet.Range["L1"].Text = "CampaignCode";
                    worksheet.Range["M1"].Text = "SourceApiExcel";

                    int a1 = 1;
                    for (int i = 0; i < ks.Count; i++)
                    {

                        a1 = a1 + 1;
                        worksheet.Range["A" + "" + a1 + ""].Text = ks[i].phone_number;
                        worksheet.Range["B" + "" + a1 + ""].Text = ks[i].province_code;
                        worksheet.Range["C" + "" + a1 + ""].Text = ks[i].bound_code;
                        worksheet.Range["D" + "" + a1 + ""].Text = a.PartnerCode;
                        worksheet.Range["E" + "" + a1 + ""].Text = "B" + ks[i].band_cc;
                        worksheet.Range["F" + "" + a1 + ""].Text = "B" + ks[i].band_upl;
                        worksheet.Range["G" + "" + a1 + ""].Text = ks[i].cc_limit;
                        worksheet.Range["H" + "" + a1 + ""].Text = ks[i].upl_limit;
                        worksheet.Range["I" + "" + a1 + ""].Text = ks[i].upl_interest;
                        worksheet.Range["J" + "" + a1 + ""].Text = ks[i].valid_date;
                        worksheet.Range["K" + "" + a1 + ""].Text = ks[i].post_date;
                        worksheet.Range["L" + "" + a1 + ""].Text = a.CampaignCode;
                        worksheet.Range["M" + "" + a1 + ""].Text = a.SourceApiExcel;
                    }

                    //Save the workbook to disk in xlsx format
                    log.Info("Excel2016");
                    workbook.Version = ExcelVersion.Excel2016;
                    try
                    {
                        workbook.SaveAs(fullPath);
                    }
                    catch (Exception ex)
                    {
                        log.Info(ex);
                        throw;
                    }
                    log.Info("save Done");

                }
                var errorMessage = "you can return the errors here!";
                //Return the Excel file name
                return Json(new { fileName = fileName, errorMessage });
            }
            catch (Exception ex)
            {

                log.Info(ex);
                throw;
            }
            return null;
        }

        [HttpGet]
        public ActionResult Download(string fileName)
        {
            //Get the temp folder and file path in server
            string fullPath = Path.Combine(Server.MapPath("~/temp"), fileName);
            byte[] fileByteArray = System.IO.File.ReadAllBytes(fullPath);
            System.IO.File.Delete(fullPath);
            return File(fileByteArray, "application/vnd.ms-excel", fileName);
        }
    }
    class LogTest
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(LogTest));

        static void Main(string[] args)
        {
            XmlConfigurator.Configure();
            log.Info("Info message");
        }

    }
}
