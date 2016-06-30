﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PerkinsMonitor.Controllers
{
    public class ListController : Controller
    {
        public ActionResult Index()
        {
			StudentDatabase db = new StudentDatabase ();

			db.Connect ();

			return View (db.SignedInStudents());
        }
    }
}
