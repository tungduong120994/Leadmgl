using Lead_MGTModel.EntityModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Lead_MGT.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            //Lead_GWEntities1 objEntity = new Lead_GWEntities1();
            //List<Lead_GW_Compare> lstEmp = new List<Lead_GW_Compare>();
            //// lstEmp = objEntity.Lead_GW_Compare.ToList();
            //var context = new Lead_GWEntities1();
            //var query = context.Lead_GW_Compare
            //                   .Where(s => s.dwh_exists == 1 && s.transfer_to == null).Take(1000).ToList()
            //                   ;
            return View();
        }
    }
}