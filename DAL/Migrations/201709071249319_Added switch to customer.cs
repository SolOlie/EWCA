namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addedswitchtocustomer : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Switches", "CustomerId", c => c.Int(nullable: false));
            CreateIndex("dbo.Switches", "CustomerId");
            AddForeignKey("dbo.Switches", "CustomerId", "dbo.Customers", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Switches", "CustomerId", "dbo.Customers");
            DropIndex("dbo.Switches", new[] { "CustomerId" });
            DropColumn("dbo.Switches", "CustomerId");
        }
    }
}
