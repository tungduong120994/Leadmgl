using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Data;
using System.Data.OleDb;
using System.Configuration;
using Lead_MGTValueObject.ImportLeadModel;
using Lead_MGTBusiness.ImportLead;
using Lead_MGTValueObject.Common;
using Lead_MGTModel.EntityModel;
using Lead_MGTValueObject.DataFillterModel;
using Lead_MGTBusiness.GetAllDataFillter;
using log4net.Config;
using log4net;
using MySql.Data.MySqlClient;

namespace Lead_MGT.Controllers
{
    public class ImportLeadController : Controller
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(LogTest));
        ImportLeadBuiness obj = new ImportLeadBuiness();
        GetAllDataFillterBuiness objGetData = new GetAllDataFillterBuiness();
        DataTableToList objCommom = new DataTableToList();
        DatatableLeadImport objData = new DatatableLeadImport();
        List<ImportLeadModel> lis = new List<ImportLeadModel>();
        List<DuAnDataFillterModel> AllDuAn = new List<DuAnDataFillterModel>();
        List<BoundCodeDataFillterModel> AllBoundCode = new List<BoundCodeDataFillterModel>();
        List<Lead_Import> lead = new List<Lead_Import>();

        private bool connection_open;
        private MySqlConnection connection;
        //private readonly IRepository<Da_DuAn> _da_DuAnRepository;
        public ActionResult Index()
        {
            


            AllDuAn = objGetData.GetAllDataDuAn();
            AllBoundCode = objGetData.GetAllDataBoundCode();

            ViewBag.DuAn = new SelectList(AllDuAn);
            ViewBag.BoundCode = new SelectList(AllBoundCode);
            lead = obj.DataLead_Import();
            return View(lead);
        }
        [HttpPost]
        public ActionResult GetDataFillter(string sanPham)
        {

            AllBoundCode = objGetData.GetDataBoundCodeBySP(sanPham);


            ViewBag.BoundCode = new SelectList(AllBoundCode);

            return View();
        }

        [HttpPost]
        public ActionResult Import(HttpPostedFileBase postedFile, string postDate, string duAn, string boundCode, string sanPham)
        {
            try
            {
              
                AllDuAn = objGetData.GetAllDataDuAn();
                AllBoundCode = objGetData.GetAllDataBoundCode();

                ViewBag.DuAn = new SelectList(AllDuAn);
                ViewBag.BoundCode = new SelectList(AllBoundCode);

                string filePath = "" + DateTime.Now + "";
                log.Info("đọc file");
                if (postedFile != null)
                {
                    string path = Server.MapPath("~/Uploads/");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    filePath = path + Path.GetFileName(postedFile.FileName);
                    string extension = Path.GetExtension(postedFile.FileName);
                    postedFile.SaveAs(filePath);
                    string conString = string.Empty;
                    switch (extension)
                    {
                        case ".xls": //Excel 97-03.
                            conString = ConfigurationManager.ConnectionStrings["Excel03ConString"].ConnectionString;
                            break;
                        case ".xlsx": //Excel 07 and above.
                            conString = ConfigurationManager.ConnectionStrings["Excel07ConString"].ConnectionString;
                            break;
                    }
                    log.Info("Lay config");
                    DataTable dt = objData.CreeateTable();
                    DataTable dtinsert = objData.CreeateTable();
                    conString = string.Format(conString, filePath);
                    log.Info("đọc DL");
                    using (OleDbConnection connExcel = new OleDbConnection(conString))
                    {
                        log.Info("1");
                        using (OleDbCommand cmdExcel = new OleDbCommand())
                        {
                            log.Info("2");
                            using (OleDbDataAdapter odaExcel = new OleDbDataAdapter())
                            {
                                log.Info("3");
                                cmdExcel.Connection = connExcel;
                                log.Info("4");
                                //Get the name of First Sheet.
                                connExcel.Open();
                                DataTable dtExcelSchema;
                                log.Info("5");
                                dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                                string sheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
                                connExcel.Close();
                                log.Info("6");
                                //Read Data from First Sheet.
                                connExcel.Open();
                                cmdExcel.CommandText = "SELECT * From [" + sheetName + "]";
                                odaExcel.SelectCommand = cmdExcel;
                                log.Info("7");
                                odaExcel.Fill(dt);
                                connExcel.Close();
                                log.Info(dt);
                            }
                        }
                    }
                    DateTime aasaa = Convert.ToDateTime(postDate);
                    log.Info("Luu DL");
                    obj.BulkInsertLead_Import(dt, duAn, sanPham, boundCode, postDate);
                    System.IO.File.Delete(filePath);

                }
                ViewBag.LastTenYears = new SelectList(AllDuAn);
                lead = obj.DataLead_Import();
                return View("Index", lead);

            }
            catch (Exception ex)
            {
                log.Info(ex);
                lead = obj.DataLead_Import();
                return View("Index", lead);
            }
            

        }
        [HttpPost]
        public ActionResult LeadGW()
        {
            try
            {
                Get_Connection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = connection;
                cmd.CommandText =
                string.Format("select * from gw_cust_lead_copy1  where transfer_to is null limit 10");
                cmd.CommandTimeout = 100000;
                MySqlDataReader reader = cmd.ExecuteReader();
                var dt = new DataTable();
                dt.Load(reader);
                connection.Close();
                obj.LeadGW(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string Date = DateTime.Now.ToString("yyyy-MM-dd");
                    Get_Connection();
                    MySqlCommand cmd1 = new MySqlCommand();
                    cmd1.Connection = connection;
                    cmd1.CommandText =
                    string.Format("update gw_cust_lead_copy1 set transfer_to ='LMGT',  transfer_date='"+Date+"' where id= "+ dt.Rows[i]["Id"].ToString() + " ");
                    cmd1.CommandTimeout = 100000;
                    MySqlDataReader reader1 = cmd1.ExecuteReader();
                    var dt1 = new DataTable();
                    dt1.Load(reader);
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                log.Info(ex);
                throw;
            }
           

           

            return null;
        }

        private void Get_Connection()
        {
            connection_open = false;
            connection = new MySqlConnection();
            connection.ConnectionString = ConfigurationManager.ConnectionStrings["MySQLConnection11"].ConnectionString;
            if (Open_Local_Connection())
            {
                connection_open = true;
            }
            else
            {

            }

        }

        private bool Open_Local_Connection()
        {
            try
            {
                connection.Open();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

    }
}
