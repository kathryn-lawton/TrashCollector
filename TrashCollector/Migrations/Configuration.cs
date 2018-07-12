namespace TrashCollector.Migrations
{
	using Microsoft.AspNet.Identity.EntityFramework;
	using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<TrashCollector.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(TrashCollector.Models.ApplicationDbContext context)
        {
			context.Roles.AddOrUpdate(
			 r => r.Name,
				 new IdentityRole { Name = "Customer" },
				 new IdentityRole { Name = "Employee" }
				 );

			context.City.AddOrUpdate(
				c => c.Name,
				new Models.City { Name = "Milwaukee" }
				);

			context.State.AddOrUpdate(
				s => s.Abbreviation,
				new Models.State { Abbreviation = "WI" }
				);

			context.Zipcode.AddOrUpdate(
				z => z.Zip,
				new Models.Zipcode { Zip = "53202" },
				new Models.Zipcode { Zip = "53203" }
				);

			context.PickupDay.AddOrUpdate(
				p => p.Name,
				new Models.PickupDay { Name = "Monday" },
				new Models.PickupDay { Name = "Tuesday" },
				new Models.PickupDay { Name = "Wednesday" },
				new Models.PickupDay { Name = "Thursday" },
				new Models.PickupDay { Name = "Friday" }
				);

			context.Pickup.AddOrUpdate(
				p => p.PickupStatus,
				new Models.Pickup { PickupStatus = false, PickupCost = 20, PickupDayId = 1, CustomerId = 1, ZipcodeId = 1 }
				);
		}
    }
}
