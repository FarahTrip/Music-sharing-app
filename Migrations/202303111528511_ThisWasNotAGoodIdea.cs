namespace Trippin_Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ThisWasNotAGoodIdea : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Likes", new[] { "UserId", "PiesaId" });
            AlterColumn("dbo.Likes", "UserId", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Likes", "UserId", c => c.Guid(nullable: false));
            CreateIndex("dbo.Likes", new[] { "UserId", "PiesaId" }, unique: true);
        }
    }
}
