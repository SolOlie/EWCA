namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Assetcontainsswitch : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Ports", "SwitchId", "dbo.Switches");
            DropPrimaryKey("dbo.Switches");
            AddColumn("dbo.Assets", "SwitchId", c => c.Int(nullable: false));
            AddColumn("dbo.Switches", "AssetId", c => c.Int(nullable: false));
            AlterColumn("dbo.Switches", "Id", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.Switches", "Id");
            CreateIndex("dbo.Switches", "Id");
            AddForeignKey("dbo.Switches", "Id", "dbo.Assets", "Id");
            AddForeignKey("dbo.Ports", "SwitchId", "dbo.Switches", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Ports", "SwitchId", "dbo.Switches");
            DropForeignKey("dbo.Switches", "Id", "dbo.Assets");
            DropIndex("dbo.Switches", new[] { "Id" });
            DropPrimaryKey("dbo.Switches");
            AlterColumn("dbo.Switches", "Id", c => c.Int(nullable: false, identity: true));
            DropColumn("dbo.Switches", "AssetId");
            DropColumn("dbo.Assets", "SwitchId");
            AddPrimaryKey("dbo.Switches", "Id");
            AddForeignKey("dbo.Ports", "SwitchId", "dbo.Switches", "Id", cascadeDelete: true);
        }
    }
}
