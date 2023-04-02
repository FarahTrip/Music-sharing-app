namespace Trippin_Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingBeatIdToLikes : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Likes", "BeatId", c => c.Guid());
            AlterColumn("dbo.Likes", "PiesaId", c => c.Guid());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Likes", "PiesaId", c => c.Guid(nullable: false));
            DropColumn("dbo.Likes", "BeatId");
        }
    }
}
