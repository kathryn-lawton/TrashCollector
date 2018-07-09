namespace TrashCollector.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addpickupday : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PickupDays",
                c => new
                    {
                        PickupDayId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.PickupDayId);
            
            AddColumn("dbo.Customers", "PickupDayId", c => c.Int(nullable: false));
            CreateIndex("dbo.Customers", "PickupDayId");
            AddForeignKey("dbo.Customers", "PickupDayId", "dbo.PickupDays", "PickupDayId", cascadeDelete: true);
            DropColumn("dbo.Customers", "PickupDay");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Customers", "PickupDay", c => c.String());
            DropForeignKey("dbo.Customers", "PickupDayId", "dbo.PickupDays");
            DropIndex("dbo.Customers", new[] { "PickupDayId" });
            DropColumn("dbo.Customers", "PickupDayId");
            DropTable("dbo.PickupDays");
        }
    }
}
