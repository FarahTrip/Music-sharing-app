namespace Trippin_Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addingDatesToPLaylists : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PlayLists", "DateCreated", c => c.DateTime());
            AddColumn("dbo.PlaylistContents", "DateAdded", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PlaylistContents", "DateAdded");
            DropColumn("dbo.PlayLists", "DateCreated");
        }
    }
}
