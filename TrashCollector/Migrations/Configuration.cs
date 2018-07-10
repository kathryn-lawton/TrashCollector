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
				new Models.State { Abbreviation = "AK" },
				new Models.State { Abbreviation = "AL" },
				new Models.State { Abbreviation = "AR" },
				new Models.State { Abbreviation = "AZ" },
				new Models.State { Abbreviation = "CA" },
				new Models.State { Abbreviation = "CO" },
				new Models.State { Abbreviation = "CT" },
				new Models.State { Abbreviation = "DE" },
				new Models.State { Abbreviation = "FL" },
				new Models.State { Abbreviation = "GA" },
				new Models.State { Abbreviation = "HI" },
				new Models.State { Abbreviation = "IA" },
				new Models.State { Abbreviation = "ID" },
				new Models.State { Abbreviation = "IL" },
				new Models.State { Abbreviation = "IN" },
				new Models.State { Abbreviation = "KS" },
				new Models.State { Abbreviation = "LA" },
				new Models.State { Abbreviation = "MA" },
				new Models.State { Abbreviation = "MD" },
				new Models.State { Abbreviation = "ME" },
				new Models.State { Abbreviation = "MI" },
				new Models.State { Abbreviation = "MN" },
				new Models.State { Abbreviation = "MO" },
				new Models.State { Abbreviation = "MS" },
				new Models.State { Abbreviation = "MT" },
				new Models.State { Abbreviation = "NC" },
				new Models.State { Abbreviation = "ND" },
				new Models.State { Abbreviation = "NE" },
				new Models.State { Abbreviation = "NH" },
				new Models.State { Abbreviation = "NJ" },
				new Models.State { Abbreviation = "NM" },
				new Models.State { Abbreviation = "NV" },
				new Models.State { Abbreviation = "NY" },
				new Models.State { Abbreviation = "OH" },
				new Models.State { Abbreviation = "OK" },
				new Models.State { Abbreviation = "OR" },
				new Models.State { Abbreviation = "PA" },
				new Models.State { Abbreviation = "RI" },
				new Models.State { Abbreviation = "SC" },
				new Models.State { Abbreviation = "SD" },
				new Models.State { Abbreviation = "TN" },
				new Models.State { Abbreviation = "TX" },
				new Models.State { Abbreviation = "UT" },
				new Models.State { Abbreviation = "VA" },
				new Models.State { Abbreviation = "VT" },
				new Models.State { Abbreviation = "WA" },
				new Models.State { Abbreviation = "WI" },
				new Models.State { Abbreviation = "WV" },
				new Models.State { Abbreviation = "WY" }
				);

			context.Zipcode.AddOrUpdate(
				z => z.Zip,
				new Models.Zipcode { Zip = "53201" },
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
		}
    }
}
