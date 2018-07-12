using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TrashCollector.Models;
using TrashCollector.ViewModels;

namespace TrashCollector.Controllers
{
    public class CustomersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

		[HttpGet]
		public ActionResult Index(string searching)
		{
			var currentUserId = User.Identity.GetUserId();
			if (User.IsInRole("Employee"))
			{
				Employee employee = db.Employee.Where(e => e.ApplicationUserID == currentUserId).FirstOrDefault();

				List<Customer> customers = new List<Customer>();
				if(!string.IsNullOrEmpty(searching))
				{
					customers = db.Customer.Where(c => c.ZipcodeId == employee.ZipcodeId && c.PickupDay.Name.Contains(searching)).Include(c => c.City).Include(c => c.State).Include(c => c.Zipcode).Include(c => c.PickupDay).ToList();
				}
				else
				{
					customers = db.Customer.Where(c => c.ZipcodeId == employee.ZipcodeId).Include(c => c.City).Include(c => c.State).Include(c => c.Zipcode).Include(c => c.PickupDay).ToList();
				}

				ViewBag.PickupDayId = new SelectList(db.PickupDay, "PickupDayId", "Name");
				return View(customers.ToList());
			}
			else if(User.IsInRole("Customer"))
			{
				var customers = db.Customer.Where(c => c.ApplicationUserID == currentUserId).Include(c => c.City).Include(c => c.State).Include(c => c.Zipcode).Include(c => c.PickupDay);
				return View(customers.ToList());
			}
			else
			{
				return RedirectToAction("Index", "Home");
			}
		}

		// GET: Customers/Details/5
		public ActionResult Details(int? id)
        {
			Customer customer;
			if(id == null)
			{
				var currentUserId = User.Identity.GetUserId();
				if (currentUserId == null)
				{
					return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
				}

				customer = db.Customer.Where(c => c.ApplicationUserID == currentUserId).Include(c => c.City).Include(c => c.State).Include(c => c.Zipcode).Include(c => c.PickupDay).FirstOrDefault();
			}
			else
			{
				customer = db.Customer.Where(c => c.CustomerId == id).Include(c => c.City).Include(c => c.State).Include(c => c.Zipcode).Include(c => c.PickupDay).FirstOrDefault();
			}

			if (customer == null)
			{
				return HttpNotFound();
			}

			return View(customer);
        }


		public ActionResult GetAddress(int? id)
		{
			var customerAddress = db.Customer.Where(c => c.CustomerId == id).Include(c => c.City).Include(c => c.State).Include(c => c.Zipcode).FirstOrDefault();
			string resultAddress = customerAddress.StreetAddress1 + " " + customerAddress.City.Name + " " + customerAddress.Zipcode.Zip;
			return Json("resultAddress", JsonRequestBehavior.AllowGet);
		}

		// GET: Customers/Create
		public ActionResult Create()
        {
			ApplicationDbContext db = new ApplicationDbContext();
			var pickupDays = db.PickupDay.ToList();
			Customer customer = new Customer();

			ViewBag.CityId = new SelectList(db.City, "CityId", "Name");
            ViewBag.StateId = new SelectList(db.State, "StateId", "Abbreviation");
            ViewBag.ZipcodeId = new SelectList(db.Zipcode, "ZipcodeId", "Zip");
			ViewBag.PickupDayId = new SelectList(db.PickupDay, "PickupDayId", "Name");
            return View(customer);
        }

        // POST: Customers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CustomerId,FirstName,LastName,EmailAddress,StreetAddress1,StreetAddress2,CityId,StateId,ZipcodeId,PickupDayId, ApplicationUserId")] Customer customer)
        {
			ApplicationDbContext db = new ApplicationDbContext();
			if (ModelState.IsValid)
            {
				customer.ApplicationUserID = User.Identity.GetUserId();
				db.Customer.Add(customer);

				Pickup pickup = new Pickup()
				{
					CustomerId = customer.CustomerId,
					PickupStatus = false,
					PickupCost = 20.00,
					PickupDayId = customer.PickupDayId,
					ZipcodeId = customer.ZipcodeId
				};
				db.Pickup.Add(pickup);

                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CityId = new SelectList(db.City, "CityId", "Name", customer.CityId);
            ViewBag.StateId = new SelectList(db.State, "StateId", "Abbreviation", customer.StateId);
            ViewBag.ZipcodeId = new SelectList(db.Zipcode, "ZipcodeId", "Zip", customer.ZipcodeId);
			ViewBag.PickupDayId = new SelectList(db.PickupDay, "PickupDayId", "Name", customer.PickupDayId);

			return View(customer);
        }

        // GET: Customers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customer.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            ViewBag.CityId = new SelectList(db.City, "CityId", "Name", customer.CityId);
            ViewBag.StateId = new SelectList(db.State, "StateId", "Abbreviation", customer.StateId);
            ViewBag.ZipcodeId = new SelectList(db.Zipcode, "ZipcodeId", "Zip", customer.ZipcodeId);
			ViewBag.PickupDayId = new SelectList(db.PickupDay, "PickupDayId", "Name", customer.PickupDayId);
			return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ApplicationUserID,CustomerId,FirstName,LastName,EmailAddress,StreetAddress1,StreetAddress2,CityId,StateId,ZipcodeId,PickupDayId,PickupStatus")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CityId = new SelectList(db.City, "CityId", "Name", customer.CityId);
            ViewBag.StateId = new SelectList(db.State, "StateId", "Abbreviation", customer.StateId);
            ViewBag.ZipcodeId = new SelectList(db.Zipcode, "ZipcodeId", "Zip", customer.ZipcodeId);
			ViewBag.PickupDayId = new SelectList(db.PickupDay, "PickupDayId", "Name", customer.PickupDayId);
			return View(customer);
        }

		// GET: Customers/AddPickupDay
		[HttpGet]
		public ActionResult AddPickupDay()
		{
			var model = new AddPickupModel();

			ViewBag.PickupDayId = new SelectList(db.PickupDay, "PickupDayId", "Name", model.PickupDayId);

			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult AddPickupDay([Bind(Include = "PickupDayId")] AddPickupModel model)
		{
			var currentUserId = User.Identity.GetUserId();
			Customer customer = db.Customer.Where(c => c.ApplicationUserID == currentUserId).Include(c => c.City).Include(c => c.State).Include(c => c.Zipcode).Include(c => c.PickupDay).FirstOrDefault();

			Pickup pickup = new Pickup();
			pickup.CustomerId = customer.CustomerId;
			pickup.PickupDayId = model.PickupDayId;
			pickup.PickupCost = 20.00;
			pickup.PickupStatus = false;
			pickup.ZipcodeId = customer.ZipcodeId;

			db.Pickup.Add(pickup);
			db.SaveChanges();

			return RedirectToAction("Index");
		}

		[HttpGet]
		public ActionResult MonthlyBill()
		{
			var currentUserId = User.Identity.GetUserId();
			Customer customer = db.Customer.Where(c => c.ApplicationUserID == currentUserId).Include(c => c.City).Include(c => c.State).Include(c => c.Zipcode).Include(c => c.PickupDay).FirstOrDefault();
			var foundPickups = db.Pickup.Where(p => p.CustomerId == customer.CustomerId && p.PickupStatus == true).Include(p =>p.PickupDay).ToList();

			double totalBill = 0;
			foreach(var pickup in foundPickups)
			{
				totalBill += pickup.PickupCost;
			}

			MonthlyBillingModel bill = new MonthlyBillingModel()
			{
				Pickups = foundPickups,
				TotalMonthlyBill = totalBill
			};

			return View(bill);
		}

		protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
