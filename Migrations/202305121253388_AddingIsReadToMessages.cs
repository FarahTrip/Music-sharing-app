namespace Trippin_Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingIsReadToMessages : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ChatContents", "IsRead", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ChatContents", "IsRead");
        }
    }
}
