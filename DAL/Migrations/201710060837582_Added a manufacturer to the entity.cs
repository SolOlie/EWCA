namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addedamanufacturertotheentity : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Manufacturers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        manufacturer = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Assets", "ManufacturerId", c => c.Int());
            AddColumn("dbo.Assets", "SerialNumber", c => c.String());
            AddColumn("dbo.Assets", "Model", c => c.String());
            CreateIndex("dbo.Assets", "ManufacturerId");
            AddForeignKey("dbo.Assets", "ManufacturerId", "dbo.Manufacturers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Assets", "ManufacturerId", "dbo.Manufacturers");
            DropIndex("dbo.Assets", new[] { "ManufacturerId" });
            DropColumn("dbo.Assets", "Model");
            DropColumn("dbo.Assets", "SerialNumber");
            DropColumn("dbo.Assets", "ManufacturerId");
            DropTable("dbo.Manufacturers");
        }
    }
}
