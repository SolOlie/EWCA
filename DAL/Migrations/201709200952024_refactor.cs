namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class refactor : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Ports", "AssetId", c => c.Int(nullable: false));
            DropColumn("dbo.Ports", "UplinkId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Ports", "UplinkId", c => c.Int(nullable: false));
            DropColumn("dbo.Ports", "AssetId");
        }
    }
}
