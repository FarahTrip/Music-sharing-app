namespace Trippin_Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingServerPathToPiese : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Piese", "S3ServerPath", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Piese", "S3ServerPath");
        }
    }
}
