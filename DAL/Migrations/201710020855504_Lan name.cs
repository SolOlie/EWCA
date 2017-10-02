namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Lanname : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Lans", "Name", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Lans", "Name");
        }
    }
}
