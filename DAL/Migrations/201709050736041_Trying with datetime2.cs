namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Tryingwithdatetime2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Changelogs", "ChangedDate", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Changelogs", "ChangedDate", c => c.DateTime(nullable: false));
        }
    }
}
