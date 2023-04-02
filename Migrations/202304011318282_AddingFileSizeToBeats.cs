namespace Trippin_Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingFileSizeToBeats : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Beats", "FileSize", c => c.Single(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Beats", "FileSize");
        }
    }
}
