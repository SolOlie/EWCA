namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ContentFilemade : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ContentFiles",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Content = c.Binary(),
                        FileId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Files", t => t.Id)
                .Index(t => t.Id);
            
            AddColumn("dbo.Files", "ContentFileId", c => c.Int(nullable: false));
            DropColumn("dbo.Files", "Content");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Files", "Content", c => c.Binary());
            DropForeignKey("dbo.ContentFiles", "Id", "dbo.Files");
            DropIndex("dbo.ContentFiles", new[] { "Id" });
            DropColumn("dbo.Files", "ContentFileId");
            DropTable("dbo.ContentFiles");
        }
    }
}
