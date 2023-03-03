namespace Trippin_Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MoreFieldsInBeats : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Beats", "IsBanger", c => c.Boolean(nullable: false));
            AddColumn("dbo.Beats", "BeatLink", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Beats", "BeatLink");
            DropColumn("dbo.Beats", "IsBanger");
        }
    }
}
