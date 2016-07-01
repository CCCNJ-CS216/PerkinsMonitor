using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PerkinsMonitor.Controllers
{
    public class HistoryController : Controller
    {
        public ActionResult Index()
        {
			StudentDatabase db = new StudentDatabase ();
			db.Connect ();

			string pw = System.IO.File.ReadAllLines ("/databases/password")[0];
			if (Request.Params.AllKeys.Contains ("password") && Request.Params ["password"].Equals (pw)) {
				return View (db.SessionHistory ());
			} else
				return View ("~/Views/Validator/Index.cshtml", new ValidationRequest ("/History", "password"));
        }
    }
}
