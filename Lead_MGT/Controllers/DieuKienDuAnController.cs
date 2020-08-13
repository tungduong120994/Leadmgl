using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Data;
using System.Data.OleDb;
using System.Configuration;
using Lead_MGTModel.EntityModel;
using Lead_MGTValueObject.ImportLeadModel;
using Lead_MGTBusiness.ImportLead;
using Lead_MGTValueObject.Common;
using Lead_MGTValueObject.DataFillterModel;
using Lead_MGTBusiness.DieuKienDuAn;
using Lead_MGTBusiness.GetAllDataFillter;
using Lead_MGTValueObject.DieuKienDuAnModel;

namespace Lead_MGT.Controllers
{
    public class DieuKienDuAnController : Controller
    {
        DieuKienDuAnBuiness obj = new DieuKienDuAnBuiness();
        GetAllDataFillterBuiness objGetData = new GetAllDataFillterBuiness();
        DataTableToList objCommom = new DataTableToList();
        DatatableLeadImport objData = new DatatableLeadImport();
        List<ImportLeadModel> lis = new List<ImportLeadModel>();
        List<DuAnDataFillterModel> AllDuAn = new List<DuAnDataFillterModel>();
        List<BoundCodeDataFillterModel> AllBoundCode = new List<BoundCodeDataFillterModel>();
        List<CD_DieuKienCheckDuAn> lead = new List<CD_DieuKienCheckDuAn>();
        List<BoundCodeDeDupModel> lisBoundCodeDeDup = new List<BoundCodeDeDupModel>();
        List<DieuKienCheckDeDupModel> lisDeDup = new List<DieuKienCheckDeDupModel>();
        public ActionResult Index()
        {
            //float a = 1000;
            //float b = 0;
            //float c = 0;
            //for (int i = 0; i < 10; i++)
            //{
            //    c = (b * 10) / 100;
            //    b = (a * 5) / 100;
               


            //    a = a - b + c;
            //}
            //float s = a;
            AllBoundCode = objGetData.GetAllDataBoundCode();
            ViewBag.BoundCode = new SelectList(AllBoundCode);
            //lead = obj.DataLead_Import();
            return View();
        }
        [HttpPost]
        public ActionResult Save(List<CachGhepModel> listLich)
        {
            obj.SaveCauHinhBoundCode(listLich);
            return null;
        }
        public ActionResult SaveCauHinh(SaveCauHinhDuAnModel cauHinh)
        {
            obj.SaveCauHinhDuAn(cauHinh);
            return null;
        }
        //public ActionResult LoadBoundCode(SaveCauHinhDuAnModel cauHinh)
        //{
        //   // obj.SaveCauHinhDuAn(cauHinh);
        //    return null;
        //}
        [HttpPost]
        public JsonResult LoadBoundCode(string HeThong)
        {
            lisBoundCodeDeDup = obj.getDataBouCodeDeDup();
           // lead = objGetData.GetDataBoundCodeBySP(HeThong);
            return Json(lisBoundCodeDeDup, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult LoadDieuKien(string HeThong)
        {
            lisDeDup = obj.getDataDieuKienCheck();
           // lead = objGetData.GetDataBoundCodeBySP(HeThong);
            return Json(lisDeDup, JsonRequestBehavior.AllowGet);
        }

    }
}
