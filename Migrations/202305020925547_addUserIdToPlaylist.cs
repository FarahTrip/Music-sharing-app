namespace Trippin_Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addUserIdToPlaylist : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PlaylistContents", "UserId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PlaylistContents", "UserId");
        }
    }
}
