namespace Trippin_Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addingUsernameToAccount : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "UsernameCont", c => c.String(nullable: false, maxLength: 100));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "UsernameCont");
        }
    }
}
