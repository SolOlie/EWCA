namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Assets",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TypeId = c.Int(),
                        Name = c.String(),
                        CustomerId = c.Int(nullable: false),
                        Description = c.String(),
                        Address = c.String(),
                        Usedby = c.String(),
                        Location = c.String(),
                        InstallationDate = c.DateTime(nullable: false),
                        FileAttachmentUrl = c.String(),
                        Login = c.String(),
                        Password = c.String(),
                        IpAddress = c.String(),
                        OS = c.String(),
                        ManagedBy_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customers", t => t.CustomerId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.ManagedBy_Id)
                .ForeignKey("dbo.AssetTypes", t => t.TypeId)
                .Index(t => t.TypeId)
                .Index(t => t.CustomerId)
                .Index(t => t.ManagedBy_Id);
            
            CreateTable(
                "dbo.Changelogs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                        Hours = c.Double(nullable: false),
                        ChangedDate = c.DateTime(nullable: false),
                        User_Id = c.Int(nullable: false),
                        Asset_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.User_Id, cascadeDelete: true)
                .ForeignKey("dbo.Assets", t => t.Asset_Id, cascadeDelete: true)
                .Index(t => t.User_Id)
                .Index(t => t.Asset_Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Email = c.String(),
                        FirstName = c.String(),
                        LastName = c.String(),
                        PhoneNumber = c.String(),
                        Password = c.String(),
                        IsContactForCustomer_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customers", t => t.IsContactForCustomer_Id)
                .Index(t => t.IsContactForCustomer_Id);
            
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Firm = c.String(),
                        Address = c.String(),
                        Date = c.DateTime(nullable: false),
                        Technichian_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.Technichian_Id)
                .Index(t => t.Technichian_Id);
            
            CreateTable(
                "dbo.AssetTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Assets", "TypeId", "dbo.AssetTypes");
            DropForeignKey("dbo.Assets", "ManagedBy_Id", "dbo.Users");
            DropForeignKey("dbo.Assets", "CustomerId", "dbo.Customers");
            DropForeignKey("dbo.Changelogs", "Asset_Id", "dbo.Assets");
            DropForeignKey("dbo.Changelogs", "User_Id", "dbo.Users");
            DropForeignKey("dbo.Customers", "Technichian_Id", "dbo.Users");
            DropForeignKey("dbo.Users", "IsContactForCustomer_Id", "dbo.Customers");
            DropIndex("dbo.Customers", new[] { "Technichian_Id" });
            DropIndex("dbo.Users", new[] { "IsContactForCustomer_Id" });
            DropIndex("dbo.Changelogs", new[] { "Asset_Id" });
            DropIndex("dbo.Changelogs", new[] { "User_Id" });
            DropIndex("dbo.Assets", new[] { "ManagedBy_Id" });
            DropIndex("dbo.Assets", new[] { "CustomerId" });
            DropIndex("dbo.Assets", new[] { "TypeId" });
            DropTable("dbo.AssetTypes");
            DropTable("dbo.Customers");
            DropTable("dbo.Users");
            DropTable("dbo.Changelogs");
            DropTable("dbo.Assets");
        }
    }
}
