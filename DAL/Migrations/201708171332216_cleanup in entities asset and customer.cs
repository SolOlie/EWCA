namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cleanupinentitiesassetandcustomer : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Customers", "Technichian_Id", "dbo.Users");
            DropForeignKey("dbo.Assets", "ManagedBy_Id", "dbo.Users");
            DropIndex("dbo.Assets", new[] { "ManagedBy_Id" });
            DropIndex("dbo.Customers", new[] { "Technichian_Id" });
            DropColumn("dbo.Assets", "ManagedBy_Id");
            DropColumn("dbo.Customers", "Technichian_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Customers", "Technichian_Id", c => c.Int());
            AddColumn("dbo.Assets", "ManagedBy_Id", c => c.Int());
            CreateIndex("dbo.Customers", "Technichian_Id");
            CreateIndex("dbo.Assets", "ManagedBy_Id");
            AddForeignKey("dbo.Assets", "ManagedBy_Id", "dbo.Users", "Id");
            AddForeignKey("dbo.Customers", "Technichian_Id", "dbo.Users", "Id");
        }
    }
}
