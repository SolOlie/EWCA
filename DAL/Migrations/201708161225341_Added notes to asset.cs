namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addednotestoasset : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Assets", "Note", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Assets", "Note");
        }
    }
}
