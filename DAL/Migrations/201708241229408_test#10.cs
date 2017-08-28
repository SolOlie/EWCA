namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class test10 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Asset", newName: "Assets");
            RenameTable(name: "dbo.Changelog", newName: "Changelogs");
            RenameTable(name: "dbo.User", newName: "Users");
            RenameTable(name: "dbo.Customer", newName: "Customers");
            RenameTable(name: "dbo.File", newName: "Files");
            RenameTable(name: "dbo.ContentFile", newName: "ContentFiles");
            RenameTable(name: "dbo.AssetType", newName: "AssetTypes");
            RenameTable(name: "dbo.AuditLog", newName: "AuditLogs");
            RenameTable(name: "dbo.AuditLogDetail", newName: "AuditLogDetails");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.AuditLogDetails", newName: "AuditLogDetail");
            RenameTable(name: "dbo.AuditLogs", newName: "AuditLog");
            RenameTable(name: "dbo.AssetTypes", newName: "AssetType");
            RenameTable(name: "dbo.ContentFiles", newName: "ContentFile");
            RenameTable(name: "dbo.Files", newName: "File");
            RenameTable(name: "dbo.Customers", newName: "Customer");
            RenameTable(name: "dbo.Users", newName: "User");
            RenameTable(name: "dbo.Changelogs", newName: "Changelog");
            RenameTable(name: "dbo.Assets", newName: "Asset");
        }
    }
}
