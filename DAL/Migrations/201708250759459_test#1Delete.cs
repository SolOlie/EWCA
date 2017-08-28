namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class test1Delete : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Assets", "SoftDelete", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Assets", "SoftDelete");
        }
    }
}
