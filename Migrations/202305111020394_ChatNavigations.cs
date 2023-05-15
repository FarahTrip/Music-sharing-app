namespace Trippin_Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChatNavigations : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ChatMembers", "Chat_Id", c => c.Guid());
            CreateIndex("dbo.ChatMembers", "Chat_Id");
            AddForeignKey("dbo.ChatMembers", "Chat_Id", "dbo.Chats", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ChatMembers", "Chat_Id", "dbo.Chats");
            DropIndex("dbo.ChatMembers", new[] { "Chat_Id" });
            DropColumn("dbo.ChatMembers", "Chat_Id");
        }
    }
}
