namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addednewpropertytoanentity : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Firewalls", "InterfaceO", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Firewalls", "InterfaceO");
        }
    }
}
