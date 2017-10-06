namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addedaliastoauser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "Emailalias", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "Emailalias");
        }
    }
}
