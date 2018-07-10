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

        // GET: Customers
        public ActionResult Index()
        {
			var currentUserId = User.Identity.GetUserId();
			var customers = db.Customer.Where(c => c.ApplicationUserID == currentUserId).Include(c => c.State).Include(c => c.Zipcode).Include(c => c.PickupDay);
            return View(customers.ToList());
        }

        // GET: Customers/Details/5
        public ActionResult Details()
        {
			var currentUserId = User.Identity.GetUserId();
			if (currentUserId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
			Customer customer = db.Customer.Where(c => c.ApplicationUserID == currentUserId).Include(c => c.City).Include(c => c.State).Include(c => c.Zipcode).Include(c => c.PickupDay).FirstOrDefault();
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // GET: Customers/Create
        public ActionResult Create()
        {
			ApplicationDbContext db = new ApplicationDbContext();
			var pickupDays = db.PickupDay.ToList();
			Customer customer = new Customer();
			{

			}

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
        public ActionResult Edit([Bind(Include = "CustomerId,FirstName,LastName,EmailAddress,StreetAddress1,StreetAddress2,CityId,StateId,ZipcodeId,PickupDayId,PickupStatus")] Customer customer)
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

        // GET: Customers/Delete/5
        public ActionResult Delete(int? id)
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
            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Customer customer = db.Customer.Find(id);
            db.Customer.Remove(customer);
            db.SaveChanges();
            return RedirectToAction("Index");
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

			db.Pickup.Add(pickup);
			db.SaveChanges();

			return RedirectToAction("Index");
		}

		// GET: Customers/AddPickupDay
		public ActionResult AddPickupDay()
		{
			var model = new AddPickupModel();

			ViewBag.PickupDayId = new SelectList(db.PickupDay, "PickupDayId", "Name", model.PickupDayId);

			return View(model);
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
