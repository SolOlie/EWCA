namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class alldatetime2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Assets", "InstallationDate", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.Customers", "Date", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Customers", "Date", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Assets", "InstallationDate", c => c.DateTime(nullable: false));
        }
    }
}
