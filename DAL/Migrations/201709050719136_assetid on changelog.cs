namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class assetidonchangelog : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Changelogs", name: "Asset_Id", newName: "AssetId");
            RenameIndex(table: "dbo.Changelogs", name: "IX_Asset_Id", newName: "IX_AssetId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Changelogs", name: "IX_AssetId", newName: "IX_Asset_Id");
            RenameColumn(table: "dbo.Changelogs", name: "AssetId", newName: "Asset_Id");
        }
    }
}
