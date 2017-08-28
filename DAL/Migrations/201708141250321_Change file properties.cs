namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Changefileproperties : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Files", name: "Asset_Id", newName: "AssetId");
            RenameIndex(table: "dbo.Files", name: "IX_Asset_Id", newName: "IX_AssetId");
            AddColumn("dbo.Files", "Content", c => c.Binary());
            AddColumn("dbo.Files", "Name", c => c.String());
            AddColumn("dbo.Files", "ContentType", c => c.String());
            DropColumn("dbo.Files", "FileString");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Files", "FileString", c => c.String());
            DropColumn("dbo.Files", "ContentType");
            DropColumn("dbo.Files", "Name");
            DropColumn("dbo.Files", "Content");
            RenameIndex(table: "dbo.Files", name: "IX_AssetId", newName: "IX_Asset_Id");
            RenameColumn(table: "dbo.Files", name: "AssetId", newName: "Asset_Id");
        }
    }
}
