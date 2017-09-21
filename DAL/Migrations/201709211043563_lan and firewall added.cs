namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class lanandfirewalladded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Firewalls",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Protocol = c.String(),
                        AllowedIps = c.String(),
                        Interface = c.String(),
                        Destination = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Lans",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Network = c.String(),
                        DhcpServer = c.String(),
                        Dns = c.String(),
                        VLan = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Lans");
            DropTable("dbo.Firewalls");
        }
    }
}
