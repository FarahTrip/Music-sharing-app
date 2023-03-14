namespace Trippin_Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingLikes : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Likes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(maxLength: 128),
                        PieseId = c.Int(nullable: false),
                        Piese_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Piese", t => t.Piese_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.Piese_Id);
            
            AddColumn("dbo.Piese", "Likes", c => c.Int(nullable: false));
            DropTable("dbo.PieseFileNames");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.PieseFileNames",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            DropForeignKey("dbo.Likes", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Likes", "Piese_Id", "dbo.Piese");
            DropIndex("dbo.Likes", new[] { "Piese_Id" });
            DropIndex("dbo.Likes", new[] { "UserId" });
            DropColumn("dbo.Piese", "Likes");
            DropTable("dbo.Likes");
        }
    }
}
