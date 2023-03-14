namespace Trippin_Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MakeLikesUnique : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Likes", "UserId", c => c.Guid(nullable: false));
            CreateIndex("dbo.Likes", new[] { "UserId", "PiesaId" }, unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.Likes", new[] { "UserId", "PiesaId" });
            AlterColumn("dbo.Likes", "UserId", c => c.String());
        }
    }
}
