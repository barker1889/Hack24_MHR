﻿using System.Web.Mvc;

namespace DataDisplay.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Feedback()
        {
            return View("AddFeedback");
        }
    }
}