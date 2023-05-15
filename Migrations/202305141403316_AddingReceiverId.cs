namespace Trippin_Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingReceiverId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ChatContents", "ReceiverId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ChatContents", "ReceiverId");
        }
    }
}
