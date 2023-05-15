namespace Trippin_Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingUsernameToChatContents : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ChatContents", "UserName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ChatContents", "UserName");
        }
    }
}
