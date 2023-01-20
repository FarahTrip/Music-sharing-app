namespace Trippin_Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DateAddedInBeaturi : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Beats", "DateAdded", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Beats", "DateAdded");
        }
    }
}
