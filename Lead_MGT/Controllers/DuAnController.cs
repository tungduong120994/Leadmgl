using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Data;
using System.Data.OleDb;
using System.Configuration;

namespace Lead_MGT.Controllers
{
    public class DuAnController : Controller
    {
        public ActionResult Index()
        {
           

            return View();
        }
    }
}
