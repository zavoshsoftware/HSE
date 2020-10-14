using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HSE.Controllers
{
    public class HomeController : Infrastructure.BaseController
    {
        // GET: Home
        public ActionResult Index()
        {
            return Redirect("/login");
        }

        [Authorize]
        public ActionResult Dashboard()
        {
            return View();
        }
    }
}