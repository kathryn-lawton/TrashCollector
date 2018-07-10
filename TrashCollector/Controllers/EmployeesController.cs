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
    public class EmployeesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

  //      // GET: Employees
		//[HttpGet]
  //      public ActionResult Index()
  //      {
  //          var employee = db.Employee.Include(e => e.Zipcode);
  //          return View(employee.ToList());
  //      }

		[HttpGet]
		public ActionResult Index(string searching)
		{
			var currentUserId = User.Identity.GetUserId();
			Employee employee = db.Employee.Where(e => e.ApplicationUserID == currentUserId).FirstOrDefault();
			if (searching == null)
			{
				var customers = db.Customer.Where(c => c.ZipcodeId == employee.ZipcodeId ).Include(c => c.City).Include(c => c.State).Include(c => c.Zipcode).Include(c => c.PickupDay);
				return View(customers.ToList());
			}
			else
			{
				var customers = db.Customer.Where(c => c.ZipcodeId == employee.ZipcodeId).Where(c => c.PickupDay.Name.Contains(searching) || searching == null).Include(c => c.City).Include(c => c.State).Include(c => c.Zipcode).Include(c => c.PickupDay);
				return View(customers.ToList());
			}


			// return View();
		}




		public ActionResult Details()
		{
			var currentUserId = User.Identity.GetUserId();
			if (currentUserId == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Employee employee = db.Employee.Where(e => e.ApplicationUserID == currentUserId).Select(e => e).Include(e => e.FirstName).Include(e => e.LastName).Include(e => e.EmailAddress).Include(e => e.Zipcode).FirstOrDefault();
			if (employee == null)
			{
				return HttpNotFound();
			}
			return View(employee);
		}

        // GET: Employees/Create
        public ActionResult Create()
        {
            ViewBag.ZipcodeId = new SelectList(db.Zipcode, "ZipcodeId", "Zip");
            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EmployeeId,FirstName,LastName,EmailAddress,ZipcodeId")] Employee employee)
        {
            if (ModelState.IsValid)
            {
				employee.ApplicationUserID = User.Identity.GetUserId();
				db.Employee.Add(employee);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ZipcodeId = new SelectList(db.Zipcode, "ZipcodeId", "Zip", employee.ZipcodeId);
            return View(employee);
        }

        // GET: Employees/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employee.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            ViewBag.ZipcodeId = new SelectList(db.Zipcode, "ZipcodeId", "Zip", employee.ZipcodeId);
            return View(employee);
        }


		// POST: Employees/Edit/5
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EmployeeId,FirstName,LastName,EmailAddress,ZipcodeId")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                db.Entry(employee).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ZipcodeId = new SelectList(db.Zipcode, "ZipcodeId", "Zip", employee.ZipcodeId);
            return View(employee);
        }

		// GET: Employees/Delete/5
		public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employee.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Employee employee = db.Employee.Find(id);
            db.Employee.Remove(employee);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

		public ActionResult Pickups(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

			var currentUserId = User.Identity.GetUserId();
			Employee employee = db.Employee.Where(c => c.ApplicationUserID == currentUserId).Include(c => c.Zipcode).FirstOrDefault();
			List<Pickup> pickups = new List<Pickup>();

			var pickupDay = db.PickupDay.Where(d => d.PickupDayId == id).FirstOrDefault();
			//var pickupZipcode = db.Zipcode.Where(z => z.ZipcodeId == id).FirstOrDefault();
			pickups.InsertRange(0, db.Pickup.Where(p => p.PickupDayId == pickupDay.PickupDayId && p.PickupStatus == false).Include(p => p.Customer).ToList());
			var customerPickups = db.Customer.Where(c => c.PickupDayId == pickupDay.PickupDayId && c.ZipcodeId == employee.ZipcodeId).Select(c => c).ToList();

			foreach(var customer in customerPickups)
			{
				var pickup = new Pickup();
				pickup.Customer = customer;
				pickup.PickupCost = 20.50;
				pickup.PickupDayId = pickupDay.PickupDayId;
			//	pickup.ZipcodeId = pickupZipcode.ZipcodeId;
				pickup.PickupStatus = false;
				pickups.Add(pickup);
			}

			var model = new EmployeePickupDayModel();
			model.CurrentDay = pickupDay;
			model.CurrentZipcode = employee.Zipcode;
			model.Pickups = pickups;

			return View(model);
		}

		[HttpGet]
		public ActionResult ConfirmPickup(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Pickup pickup = db.Pickup.Find(id);
			if (pickup == null)
			{
				return HttpNotFound();
			}
			ViewBag.PickupId = new SelectList(db.Pickup, "PickupId", "PickupStatus", pickup.PickupId);
			return View(pickup);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult ConfirmPickup([Bind(Include = "PickupId, PickupStatus, PickupCost, PickupDayId, CustomerId, ZipcodeId")] Pickup pickup)
		{
			if (ModelState.IsValid)
			{
				db.Entry(pickup).State = EntityState.Modified;
				db.SaveChanges();
				return RedirectToAction("Index");
			}
			ViewBag.PickupId = new SelectList(db.Pickup, "PickupId", "PickupStatus", pickup.PickupId);
			return View(pickup);
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
