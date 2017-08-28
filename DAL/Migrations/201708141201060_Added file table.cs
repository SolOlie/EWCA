namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addedfiletable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Files",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FileString = c.String(),
                        Asset_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Assets", t => t.Asset_Id, cascadeDelete: true)
                .Index(t => t.Asset_Id);
            
            DropColumn("dbo.Assets", "FileAttachmentUrl");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Assets", "FileAttachmentUrl", c => c.String());
            DropForeignKey("dbo.Files", "Asset_Id", "dbo.Assets");
            DropIndex("dbo.Files", new[] { "Asset_Id" });
            DropTable("dbo.Files");
        }
    }
}
