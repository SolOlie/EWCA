namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixingbackupLog : DbMigration
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
            
        }
        
        public override void Down()
        {
            DropTable("dbo.BackupChangelogs");
        }
    }
}
