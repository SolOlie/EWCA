namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Tryingagain : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BackupChangelogs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EntityName = c.String(),
                        PropertyName = c.String(),
                        PrimaryKey = c.String(),
                        PrimaryKeyValue = c.String(),
                        OldValue = c.String(),
                        NewValue = c.String(),
                        DateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            DropColumn("dbo.Assets", "CreatedBy");
            DropColumn("dbo.Assets", "CreatedDate");
            DropColumn("dbo.Assets", "ModifiedBy");
            DropColumn("dbo.Assets", "DateModified");
            DropColumn("dbo.Changelogs", "CreatedBy");
            DropColumn("dbo.Changelogs", "CreatedDate");
            DropColumn("dbo.Changelogs", "ModifiedBy");
            DropColumn("dbo.Changelogs", "DateModified");
            DropColumn("dbo.Users", "CreatedBy");
            DropColumn("dbo.Users", "CreatedDate");
            DropColumn("dbo.Users", "ModifiedBy");
            DropColumn("dbo.Users", "DateModified");
            DropColumn("dbo.Customers", "CreatedBy");
            DropColumn("dbo.Customers", "CreatedDate");
            DropColumn("dbo.Customers", "ModifiedBy");
            DropColumn("dbo.Customers", "DateModified");
            DropColumn("dbo.Files", "CreatedBy");
            DropColumn("dbo.Files", "CreatedDate");
            DropColumn("dbo.Files", "ModifiedBy");
            DropColumn("dbo.Files", "DateModified");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Files", "DateModified", c => c.DateTime());
            AddColumn("dbo.Files", "ModifiedBy", c => c.Int());
            AddColumn("dbo.Files", "CreatedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Files", "CreatedBy", c => c.Int(nullable: false));
            AddColumn("dbo.Customers", "DateModified", c => c.DateTime());
            AddColumn("dbo.Customers", "ModifiedBy", c => c.Int());
            AddColumn("dbo.Customers", "CreatedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Customers", "CreatedBy", c => c.Int(nullable: false));
            AddColumn("dbo.Users", "DateModified", c => c.DateTime());
            AddColumn("dbo.Users", "ModifiedBy", c => c.Int());
            AddColumn("dbo.Users", "CreatedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Users", "CreatedBy", c => c.Int(nullable: false));
            AddColumn("dbo.Changelogs", "DateModified", c => c.DateTime());
            AddColumn("dbo.Changelogs", "ModifiedBy", c => c.Int());
            AddColumn("dbo.Changelogs", "CreatedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Changelogs", "CreatedBy", c => c.Int(nullable: false));
            AddColumn("dbo.Assets", "DateModified", c => c.DateTime());
            AddColumn("dbo.Assets", "ModifiedBy", c => c.Int());
            AddColumn("dbo.Assets", "CreatedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Assets", "CreatedBy", c => c.Int(nullable: false));
            DropTable("dbo.BackupChangelogs");
        }
    }
}
