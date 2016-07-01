using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Data.SQLite;

namespace PerkinsMonitor.Controllers
{
    public class SignInController : Controller
    {
        public ActionResult Index()
        {
            return View ();
        }

		/// <summary>
		/// Looks up a student in the database using their studentID number
		/// </summary>
		/// <param name="studentID">Student I.</param>
		public ActionResult Lookup(int studentID)
		{
			return View ("Home");
		}

		/// <summary>
		/// Gets some information about the student from the database (if any exists),
		/// and sends it as JSON to the requestor. 
		/// </summary>
		/// <returns>The JSON information about the student, including Name (first last) and major</returns>
		public string GetStudent()
		{
			int requestID = int.Parse (Request.Params ["studentID"]);

			StudentDatabase db = new StudentDatabase ();
			db.Connect ();

			Student requestedStudent = db.RetrieveStudent (requestID);

			db.Disconnect ();

			return requestedStudent.ToJSON ();
		}

		/// <summary>
		/// Signs in a student, optionally updating the database if it has updated fields.
		/// </summary>
		/// <returns>The SignIn page</returns>
		public ActionResult SignIn(string ID, string name, string major, string machineNumber)
		{

			/* Sanitize: Name*/
			if (Request.Params.AllKeys.Contains ("name") && Request.Params ["name"].Contains (" ")) {
				/* Sanitize: Major */
				if (Request.Params.AllKeys.Contains ("major")) {
					/*Sanitize: MachineNumber */
					if (Request.Params.AllKeys.Contains ("machineNumber")) {
						StudentDatabase db = new StudentDatabase ();
						db.Connect ();

						db.SignIn(
							int.Parse(Request.Params["ID"]), //ID
							Request.Params["name"].Split(' ')[0], //First
							Request.Params["name"].Split(' ')[1], //Last
							Request.Params["major"], // Major
							int.Parse(Request.Params["machineNumber"]), //MachineNumber
							(Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds); //TimeIn is now

						db.Disconnect ();

						return View ("~/Views/Home/Index.cshtml", new Warning(""));
					}
				}
			}

			return View ("~/Views/SignIn/Index.cshtml", "Warning: Improper Value Detected");
		}
    }
}
