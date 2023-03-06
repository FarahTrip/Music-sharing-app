namespace Trippin_Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserIDToPiese : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Piese", "UserId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Piese", "UserId");
        }
    }
}
