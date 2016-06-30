using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PerkinsMonitor.Controllers
{
    public class SignOutController : Controller
    {
        public ActionResult Index()
        {
            return View ();
        }

		/// <summary>
		/// Signs a student out of the lab, both removing them from the loggedIn table
		/// and adding their session to the history table
		/// </summary>
		/// <returns>The out.</returns>
		public ActionResult SignOut()
		{
			StudentDatabase db = new StudentDatabase ();
			int IDnumber = int.Parse (Request.Params ["ID"]);

			db.Connect ();

			db.SignOut (IDnumber);
			IEnumerable<Session> students = db.SignedInStudents ();
			db.Disconnect ();

			return View ("~/Views/Home/Index.cshtml", students);
		}
    }
}
