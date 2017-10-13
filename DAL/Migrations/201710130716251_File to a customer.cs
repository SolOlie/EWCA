namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Filetoacustomer : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CustomerFiles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CustomerContentTypeId = c.Int(nullable: false),
                        CustomerId = c.Int(nullable: false),
                        Name = c.String(),
                        ContentType = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customers", t => t.CustomerId, cascadeDelete: true)
                .Index(t => t.CustomerId);
            
            CreateTable(
                "dbo.CustomerContentTypes",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Content = c.Binary(),
                        FileId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CustomerFiles", t => t.Id)
                .Index(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CustomerFiles", "CustomerId", "dbo.Customers");
            DropForeignKey("dbo.CustomerContentTypes", "Id", "dbo.CustomerFiles");
            DropIndex("dbo.CustomerContentTypes", new[] { "Id" });
            DropIndex("dbo.CustomerFiles", new[] { "CustomerId" });
            DropTable("dbo.CustomerContentTypes");
            DropTable("dbo.CustomerFiles");
        }
    }
}
