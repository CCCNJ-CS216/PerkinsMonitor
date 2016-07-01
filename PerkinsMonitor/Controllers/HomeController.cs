using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;

namespace PerkinsMonitor.Controllers
{
	public class HomeController : Controller
	{
		/// <summary>
		/// Return the basic web page with sign-in and sign-out buttons
		/// </summary>
		public ActionResult Index ()
		{
			var mvcName = typeof(Controller).Assembly.GetName ();
			var isMono = Type.GetType ("Mono.Runtime") != null;

			ViewData ["Version"] = mvcName.Version.Major + "." + mvcName.Version.Minor;
			ViewData ["Runtime"] = isMono ? "Mono" : ".NET";

			return View (new Warning(""));
		}

		/// <summary>
		/// Return the Sign in page
		/// </summary>
		/// <returns>The sign in.</returns>
		public ActionResult RequestSignIn()
		{
			return View ("~/Views/SignIn/Index.cshtml");
		}

		/// <summary>
		/// Return the Sign out page
		/// </summary>
		/// <returns>The sign out.</returns>
		public ActionResult RequestSignOut()
		{
			StudentDatabase db = new StudentDatabase ();
			db.Connect ();

			return View ("~/Views/SignOut/Index.cshtml", db.SignedInStudents());
		}
	}
}

