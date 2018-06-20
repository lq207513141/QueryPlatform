using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QueryPlatform.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            Response.Redirect("~/Loom/LoomAnalysis");
            return View();
        }
    }
}
