namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addeduserIdtoentity : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Changelogs", name: "User_Id", newName: "UserId");
            RenameIndex(table: "dbo.Changelogs", name: "IX_User_Id", newName: "IX_UserId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Changelogs", name: "IX_UserId", newName: "IX_User_Id");
            RenameColumn(table: "dbo.Changelogs", name: "UserId", newName: "User_Id");
        }
    }
}
