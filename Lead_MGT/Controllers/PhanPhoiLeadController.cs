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
using Lead_MGTValueObject.PhanPhoiLeadModel;
using Lead_MGTValueObject.DataFillterModel;
using Lead_MGTBusiness.GetAllDataFillter;

namespace Lead_MGT.Controllers
{
    public class PhanPhoiLeadController : Controller
    {
        //ImportLeadBuiness obj = new ImportLeadBuiness();
        PhanPhoiLeadBuiness objGetData = new PhanPhoiLeadBuiness();
        DataTableToList objCommom = new DataTableToList();
        DatatableLeadImport objData = new DatatableLeadImport();
        List<ImportLeadModel> lis = new List<ImportLeadModel>();
        List<DuAnDataFillterModel> AllDuAn = new List<DuAnDataFillterModel>();
        List<TinhThanhModel> AllTinhThanh = new List<TinhThanhModel>();
        List<ChienDichModel> AllChienDich = new List<ChienDichModel>();
        List<BoundCodeDataFillterModel> AllBoundCode = new List<BoundCodeDataFillterModel>();
       
        List<ViewPhanPhoiLeadModel> lead = new List<ViewPhanPhoiLeadModel>();
        //private readonly IRepository<Da_DuAn> _da_DuAnRepository;
        public ActionResult Index()
        {
            AllChienDich = objGetData.GetAllChienDich();
            AllTinhThanh = objGetData.GetAllDataProvice();
            AllBoundCode = objGetData.GetAllDataBoundCode();
            AllDuAn = objGetData.GetAllDataDuAn();
            ViewBag.DuAn = new SelectList(AllDuAn);
            ViewBag.DuAn = new SelectList(AllDuAn);
            ViewBag.ChienDich = new SelectList(AllChienDich);
            ViewBag.TinhThanh = new SelectList(AllTinhThanh);
            ViewBag.BoundCode = new SelectList(AllBoundCode);
            //lead = objGetData.GetDataBoundCodeBySP("a");
           // AuToCheckPhanPhoi();
            return View(lead);
        }
        [HttpPost]
        public JsonResult GetDataBySystem(string HeThong ,string MaChienDich)
        {
            lead = objGetData.GetDataBoundCodeBySP(HeThong, MaChienDich);
            return Json(lead, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult Save(SavePhanPhoiLeadModel a)
        {
            objGetData.Save(a);


            return null;
        }
        [HttpPost]
        public ActionResult AuToCheckPhanPhoi()
        {
            objGetData.LayLead();


            return null;
        }
    }
}