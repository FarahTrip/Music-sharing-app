namespace Trippin_Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addingChatModels : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ChatContents",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Message = c.String(),
                        SenderId = c.String(),
                        SentAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ChatMembers",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        MemberId = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Chats",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Chats");
            DropTable("dbo.ChatMembers");
            DropTable("dbo.ChatContents");
        }
    }
}
