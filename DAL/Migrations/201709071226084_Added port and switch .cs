namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addedportandswitch : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Ports",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SwitchId = c.Int(nullable: false),
                        PortNumber = c.Int(nullable: false),
                        UplinkId = c.Int(nullable: false),
                        Trunk = c.String(),
                        VLAN = c.String(),
                        Note = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Switches", t => t.SwitchId, cascadeDelete: true)
                .Index(t => t.SwitchId);
            
            CreateTable(
                "dbo.Switches",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Assets", "RAM", c => c.String());
            AddColumn("dbo.Assets", "HDD", c => c.String());
            AddColumn("dbo.Assets", "PortId", c => c.Int(nullable: false));
            AddColumn("dbo.Assets", "Port_Id", c => c.Int());
            CreateIndex("dbo.Assets", "Port_Id");
            AddForeignKey("dbo.Assets", "Port_Id", "dbo.Ports", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Assets", "Port_Id", "dbo.Ports");
            DropForeignKey("dbo.Ports", "SwitchId", "dbo.Switches");
            DropIndex("dbo.Ports", new[] { "SwitchId" });
            DropIndex("dbo.Assets", new[] { "Port_Id" });
            DropColumn("dbo.Assets", "Port_Id");
            DropColumn("dbo.Assets", "PortId");
            DropColumn("dbo.Assets", "HDD");
            DropColumn("dbo.Assets", "RAM");
            DropTable("dbo.Switches");
            DropTable("dbo.Ports");
        }
    }
}
