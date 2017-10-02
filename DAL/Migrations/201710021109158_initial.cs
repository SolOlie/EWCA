namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
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
                        InstallationDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        Login = c.String(),
                        Password = c.String(),
                        IpAddress = c.String(),
                        OS = c.String(),
                        Note = c.String(),
                        SoftDelete = c.Boolean(nullable: false),
                        RAM = c.String(),
                        HDD = c.String(),
                        SwitchId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customers", t => t.CustomerId, cascadeDelete: true)
                .ForeignKey("dbo.AssetTypes", t => t.TypeId)
                .Index(t => t.TypeId)
                .Index(t => t.CustomerId);
            
            CreateTable(
                "dbo.Changelogs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        Description = c.String(),
                        AssetId = c.Int(nullable: false),
                        Minutes = c.Int(nullable: false),
                        ChangedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.Assets", t => t.AssetId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.AssetId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Email = c.String(nullable: false),
                        FirstName = c.String(),
                        LastName = c.String(),
                        PhoneNumber = c.String(),
                        Password = c.String(nullable: false),
                        IsAdmin = c.Boolean(nullable: false),
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
                        Date = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Firewalls",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Protocol = c.String(),
                        AllowedIps = c.String(),
                        Interface = c.String(),
                        Destination = c.String(),
                        CustomerId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customers", t => t.CustomerId, cascadeDelete: true)
                .Index(t => t.CustomerId);
            
            CreateTable(
                "dbo.Lans",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Network = c.String(),
                        DhcpServer = c.String(),
                        Dns = c.String(),
                        VLan = c.String(),
                        CustomerId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customers", t => t.CustomerId, cascadeDelete: true)
                .Index(t => t.CustomerId);
            
            CreateTable(
                "dbo.Switches",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Name = c.String(),
                        CustomerId = c.Int(nullable: false),
                        AssetId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customers", t => t.CustomerId, cascadeDelete: true)
                .ForeignKey("dbo.Assets", t => t.Id)
                .Index(t => t.Id)
                .Index(t => t.CustomerId);
            
            CreateTable(
                "dbo.Ports",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SwitchId = c.Int(nullable: false),
                        PortNumber = c.Int(nullable: false),
                        Asset = c.String(),
                        Trunk = c.String(),
                        VLAN = c.String(),
                        Note = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Switches", t => t.SwitchId, cascadeDelete: true)
                .Index(t => t.SwitchId);
            
            CreateTable(
                "dbo.Files",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ContentFileId = c.Int(nullable: false),
                        AssetId = c.Int(nullable: false),
                        Name = c.String(),
                        ContentType = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Assets", t => t.AssetId, cascadeDelete: true)
                .Index(t => t.AssetId);
            
            CreateTable(
                "dbo.ContentFiles",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Content = c.Binary(),
                        FileId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Files", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.AssetTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AuditLogs",
                c => new
                    {
                        AuditLogId = c.Long(nullable: false, identity: true),
                        UserName = c.String(),
                        EventDateUTC = c.DateTime(nullable: false),
                        EventType = c.Int(nullable: false),
                        TypeFullName = c.String(nullable: false, maxLength: 512),
                        RecordId = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.AuditLogId);
            
            CreateTable(
                "dbo.AuditLogDetails",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        PropertyName = c.String(nullable: false, maxLength: 256),
                        OriginalValue = c.String(),
                        NewValue = c.String(),
                        AuditLogId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AuditLogs", t => t.AuditLogId, cascadeDelete: true)
                .Index(t => t.AuditLogId);
            
            CreateTable(
                "dbo.LogMetadata",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        AuditLogId = c.Long(nullable: false),
                        Key = c.String(),
                        Value = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AuditLogs", t => t.AuditLogId, cascadeDelete: true)
                .Index(t => t.AuditLogId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.LogMetadata", "AuditLogId", "dbo.AuditLogs");
            DropForeignKey("dbo.AuditLogDetails", "AuditLogId", "dbo.AuditLogs");
            DropForeignKey("dbo.Assets", "TypeId", "dbo.AssetTypes");
            DropForeignKey("dbo.Switches", "Id", "dbo.Assets");
            DropForeignKey("dbo.Files", "AssetId", "dbo.Assets");
            DropForeignKey("dbo.ContentFiles", "Id", "dbo.Files");
            DropForeignKey("dbo.Assets", "CustomerId", "dbo.Customers");
            DropForeignKey("dbo.Changelogs", "AssetId", "dbo.Assets");
            DropForeignKey("dbo.Changelogs", "UserId", "dbo.Users");
            DropForeignKey("dbo.Switches", "CustomerId", "dbo.Customers");
            DropForeignKey("dbo.Ports", "SwitchId", "dbo.Switches");
            DropForeignKey("dbo.Lans", "CustomerId", "dbo.Customers");
            DropForeignKey("dbo.Firewalls", "CustomerId", "dbo.Customers");
            DropForeignKey("dbo.Users", "IsContactForCustomer_Id", "dbo.Customers");
            DropIndex("dbo.LogMetadata", new[] { "AuditLogId" });
            DropIndex("dbo.AuditLogDetails", new[] { "AuditLogId" });
            DropIndex("dbo.ContentFiles", new[] { "Id" });
            DropIndex("dbo.Files", new[] { "AssetId" });
            DropIndex("dbo.Ports", new[] { "SwitchId" });
            DropIndex("dbo.Switches", new[] { "CustomerId" });
            DropIndex("dbo.Switches", new[] { "Id" });
            DropIndex("dbo.Lans", new[] { "CustomerId" });
            DropIndex("dbo.Firewalls", new[] { "CustomerId" });
            DropIndex("dbo.Users", new[] { "IsContactForCustomer_Id" });
            DropIndex("dbo.Changelogs", new[] { "AssetId" });
            DropIndex("dbo.Changelogs", new[] { "UserId" });
            DropIndex("dbo.Assets", new[] { "CustomerId" });
            DropIndex("dbo.Assets", new[] { "TypeId" });
            DropTable("dbo.LogMetadata");
            DropTable("dbo.AuditLogDetails");
            DropTable("dbo.AuditLogs");
            DropTable("dbo.AssetTypes");
            DropTable("dbo.ContentFiles");
            DropTable("dbo.Files");
            DropTable("dbo.Ports");
            DropTable("dbo.Switches");
            DropTable("dbo.Lans");
            DropTable("dbo.Firewalls");
            DropTable("dbo.Customers");
            DropTable("dbo.Users");
            DropTable("dbo.Changelogs");
            DropTable("dbo.Assets");
        }
    }
}
