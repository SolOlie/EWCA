namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _float : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Changelogs", "Minutes", c => c.Single(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Changelogs", "Minutes", c => c.Double(nullable: false));
        }
    }
}
