namespace Test.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addUserRoles : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "UserRole", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "UserRole");
        }
    }
}
