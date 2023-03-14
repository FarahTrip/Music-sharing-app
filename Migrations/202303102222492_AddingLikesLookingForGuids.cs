namespace Trippin_Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingLikesLookingForGuids : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Likes", "Piese_Id", "dbo.Piese");
            DropForeignKey("dbo.Likes", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.Likes", new[] { "UserId" });
            DropIndex("dbo.Likes", new[] { "Piese_Id" });
            AddColumn("dbo.Likes", "PiesaId", c => c.Guid(nullable: false));
            AlterColumn("dbo.Likes", "UserId", c => c.String());
            DropColumn("dbo.Likes", "PieseId");
            DropColumn("dbo.Likes", "Piese_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Likes", "Piese_Id", c => c.Guid());
            AddColumn("dbo.Likes", "PieseId", c => c.Int(nullable: false));
            AlterColumn("dbo.Likes", "UserId", c => c.String(maxLength: 128));
            DropColumn("dbo.Likes", "PiesaId");
            CreateIndex("dbo.Likes", "Piese_Id");
            CreateIndex("dbo.Likes", "UserId");
            AddForeignKey("dbo.Likes", "UserId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Likes", "Piese_Id", "dbo.Piese", "Id");
        }
    }
}
