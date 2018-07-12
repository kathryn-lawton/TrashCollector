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

		// GET: Employees
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
		}

		public ActionResult Details(int? id)
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
			return View(pickup);
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

		[HttpGet]
		public ActionResult Pickups(int? PickupDayId)
		{
			if (PickupDayId == null)
			{
				PickupDayId = (int)DateTime.Now.DayOfWeek;

				if(DateTime.Now.DayOfWeek == DayOfWeek.Sunday || DateTime.Now.DayOfWeek == DayOfWeek.Saturday)
				{
					return HttpNotFound();
				}
			}

			var currentUserId = User.Identity.GetUserId();
			Employee employee = db.Employee.Where(c => c.ApplicationUserID == currentUserId).Include(c => c.Zipcode).FirstOrDefault();
			if(employee == null)
			{
				return HttpNotFound();
			}

			List<Pickup> pickups = new List<Pickup>();

			var pickupDay = db.PickupDay.Where(d => d.PickupDayId == PickupDayId).FirstOrDefault();
			pickups.InsertRange(0, db.Pickup.Where(p => p.PickupDayId == pickupDay.PickupDayId && p.PickupStatus == false && p.ZipcodeId == employee.ZipcodeId).Include(p => p.Customer).ToList());
			var customerPickups = db.Customer.Where(c => c.PickupDayId == pickupDay.PickupDayId && c.ZipcodeId == employee.ZipcodeId).Select(c => c).ToList();

			var model = new EmployeePickupDayModel();
			model.CurrentDay = pickupDay;
			model.CurrentZipcode = employee.Zipcode;
			model.Pickups = pickups;

			return View(model);
		}

		[HttpPost, ActionName("Pickups")]
		[ValidateAntiForgeryToken]
		public ActionResult Pickups(int id)
		{
			Pickup pickup = db.Pickup.Find(id);
			if (pickup == null)
			{
				return HttpNotFound();
			}

			Employee employee = db.Employee.Find(id);
			db.Employee.Remove(employee);
			db.SaveChanges();
			return RedirectToAction("Index");
		}

		[HttpGet]
		public ActionResult ConfirmPickup(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

			Pickup pickup = db.Pickup.Where(p => p.PickupId == id).Include(p => p.Customer).FirstOrDefault();
			if (pickup == null)
			{
				return HttpNotFound();
			}

			pickup.PickupStatus = true;
			db.SaveChanges();

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
