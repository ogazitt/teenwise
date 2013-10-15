namespace TeenWise.WebSite.Controllers
{
    using System;
    using System.Net;
    using System.Net.Mail;
    using System.Web.Mvc;
    using System.Web.Security;
    using TeenWise.WebSite.Models;
    using TeenWise.WebSite.Models.Storage;

    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }     

    }
}
