using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TrashCollector.Models;

namespace TrashCollector.Controllers
{
    public class CustomersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Customers
        public ActionResult Index()
        {
            var customer = db.Customer.Include(c => c.City).Include(c => c.State).Include(c => c.Zipcode).Include(c => c.PickupDay);
            return View(customer.ToList());
        }

        // GET: Customers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
			var customer = db.Customer.Where(c => c.CustomerId == id).Include(c => c.City).Include(c => c.State).Include(c => c.Zipcode).Include(c => c.PickupDay).FirstOrDefault();
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
        public ActionResult Create([Bind(Include = "CustomerId,FirstName,LastName,EmailAddress,StreetAddress1,StreetAddress2,CityId,StateId,ZipcodeId,PickupDayId")] Customer customer)
        {
			ApplicationDbContext db = new ApplicationDbContext();
			if (ModelState.IsValid)
            {
				customer.PickupStatus = false;
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
