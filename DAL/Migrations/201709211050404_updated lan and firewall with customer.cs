namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatedlanandfirewallwithcustomer : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Firewalls", "CustomerId", c => c.Int(nullable: false));
            AddColumn("dbo.Lans", "CustomerId", c => c.Int(nullable: false));
            CreateIndex("dbo.Firewalls", "CustomerId");
            CreateIndex("dbo.Lans", "CustomerId");
            AddForeignKey("dbo.Firewalls", "CustomerId", "dbo.Customers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Lans", "CustomerId", "dbo.Customers", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Lans", "CustomerId", "dbo.Customers");
            DropForeignKey("dbo.Firewalls", "CustomerId", "dbo.Customers");
            DropIndex("dbo.Lans", new[] { "CustomerId" });
            DropIndex("dbo.Firewalls", new[] { "CustomerId" });
            DropColumn("dbo.Lans", "CustomerId");
            DropColumn("dbo.Firewalls", "CustomerId");
        }
    }
}
