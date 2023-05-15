namespace Trippin_Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addingChatIdToChatContents : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ChatContents", "ChatId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ChatContents", "ChatId");
        }
    }
}
