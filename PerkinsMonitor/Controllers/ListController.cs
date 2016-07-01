using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.IO;
namespace PerkinsMonitor.Controllers
{
    public class ListController : Controller
    {
        public ActionResult Index()
        {
			string pw = System.IO.File.ReadAllLines ("/databases/password")[0];
			if (Request.Params.AllKeys.Contains ("password") && Request.Params ["password"].Equals (pw)) {
				StudentDatabase db = new StudentDatabase ();

				db.Connect ();

				return View (db.SignedInStudents ());
			} else {
				return View ("~/Views/Validator/Index.cshtml", new ValidationRequest("/List", "password"));
			}
        }
    }
}
