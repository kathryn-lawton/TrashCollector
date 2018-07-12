using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TrashCollector.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Index()
		{
			if (User.IsInRole("Employee"))
			{
				return RedirectToAction("Pickups", "Employees");
			}
			else if (User.IsInRole("Customer"))
			{
				return RedirectToAction("Details", "Customers");
			}

			return View();
		}

		public ActionResult About()
		{
			ViewBag.Message = "Your application description page.";

			return View();
		}

		public ActionResult Contact()
		{
			ViewBag.Message = "Your contact page.";

			return View();
		}
	}
}