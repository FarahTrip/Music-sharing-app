namespace Trippin_Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ForgotTheChatId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ChatMembers", "ChatId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ChatMembers", "ChatId");
        }
    }
}
