namespace Trippin_Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingNameToPlaylist : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PlayLists", "Name", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PlayLists", "Name");
        }
    }
}
