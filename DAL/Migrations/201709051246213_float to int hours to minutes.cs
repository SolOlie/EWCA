namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class floattointhourstominutes : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Changelogs", "Minutes", c => c.Int(nullable: false));
            DropColumn("dbo.Changelogs", "Hours");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Changelogs", "Hours", c => c.Single(nullable: false));
            DropColumn("dbo.Changelogs", "Minutes");
        }
    }
}
