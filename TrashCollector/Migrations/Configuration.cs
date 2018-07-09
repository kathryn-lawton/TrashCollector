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
				new Models.Zipcode { Zip = "53203" },
				new Models.Zipcode { Zip = "53204" },
				new Models.Zipcode { Zip = "53205" },
				new Models.Zipcode { Zip = "53206" },
				new Models.Zipcode { Zip = "53207" },
				new Models.Zipcode { Zip = "53208" },
				new Models.Zipcode { Zip = "53209" },
				new Models.Zipcode { Zip = "53210" },
				new Models.Zipcode { Zip = "53211" },
				new Models.Zipcode { Zip = "53212" },
				new Models.Zipcode { Zip = "53213" },
				new Models.Zipcode { Zip = "53214" },
				new Models.Zipcode { Zip = "53215" },
				new Models.Zipcode { Zip = "53216" },
				new Models.Zipcode { Zip = "53218" },
				new Models.Zipcode { Zip = "53219" },
				new Models.Zipcode { Zip = "53220" },
				new Models.Zipcode { Zip = "53221" },
				new Models.Zipcode { Zip = "53222" },
				new Models.Zipcode { Zip = "53223" },
				new Models.Zipcode { Zip = "53224" },
				new Models.Zipcode { Zip = "53225" },
				new Models.Zipcode { Zip = "53226" },
				new Models.Zipcode { Zip = "53227" },
				new Models.Zipcode { Zip = "53228" },
				new Models.Zipcode { Zip = "53233" },
				new Models.Zipcode { Zip = "53234" },
				new Models.Zipcode { Zip = "53237" },
				new Models.Zipcode { Zip = "53259" },
				new Models.Zipcode { Zip = "53263" },
				new Models.Zipcode { Zip = "53267" },
				new Models.Zipcode { Zip = "53268" },
				new Models.Zipcode { Zip = "53274" },
				new Models.Zipcode { Zip = "53278" },
				new Models.Zipcode { Zip = "53288" },
				new Models.Zipcode { Zip = "53290" },
				new Models.Zipcode { Zip = "53293" },
				new Models.Zipcode { Zip = "53295" }
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
