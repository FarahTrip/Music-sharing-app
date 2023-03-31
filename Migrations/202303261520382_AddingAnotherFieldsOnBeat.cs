namespace Trippin_Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingAnotherFieldsOnBeat : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Beats", "FileName", c => c.String());
            AddColumn("dbo.Beats", "S3ServerPath", c => c.String());
            AddColumn("dbo.Beats", "UserId", c => c.String());
            AddColumn("dbo.Beats", "Likes", c => c.Int(nullable: false));
            DropColumn("dbo.Beats", "BeatLink");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Beats", "BeatLink", c => c.String());
            DropColumn("dbo.Beats", "Likes");
            DropColumn("dbo.Beats", "UserId");
            DropColumn("dbo.Beats", "S3ServerPath");
            DropColumn("dbo.Beats", "FileName");
        }
    }
}
