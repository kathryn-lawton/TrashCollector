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
				new IdentityRole { Name = "Employee"}
				);
		}
	
    }
}
