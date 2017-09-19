namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class testAnno : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Ports", "SwitchId", "dbo.Switches");
            DropIndex("dbo.Switches", new[] { "Id" });
            DropPrimaryKey("dbo.Switches");
            AlterColumn("dbo.Switches", "Id", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.Switches", "Id");
            CreateIndex("dbo.Switches", "Id");
            AddForeignKey("dbo.Ports", "SwitchId", "dbo.Switches", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Ports", "SwitchId", "dbo.Switches");
            DropIndex("dbo.Switches", new[] { "Id" });
            DropPrimaryKey("dbo.Switches");
            AlterColumn("dbo.Switches", "Id", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.Switches", "Id");
            CreateIndex("dbo.Switches", "Id");
            AddForeignKey("dbo.Ports", "SwitchId", "dbo.Switches", "Id", cascadeDelete: true);
        }
    }
}
