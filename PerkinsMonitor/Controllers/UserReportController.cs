using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PerkinsMonitor.Controllers
{
    public class UserReportController : Controller
    {
        public ActionResult Index()
        {
			if(Request.Params.AllKeys.Contains("ID")){
				StudentDatabase db = new StudentDatabase ();
				db.Connect ();

				int ID = 000000;
				ID = int.Parse (Request.Params["ID"]);
					
				return View ("~/Views/History/Index.cshtml", db.SessionHistory(ID));
			}else 
				return View("~/Views/Home/Index.cshtml", new Warning("You must specify which ID you want"));
        }
    }
}
