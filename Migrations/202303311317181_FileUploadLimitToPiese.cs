namespace Trippin_Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FileUploadLimitToPiese : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Piese", "FileUploadHardLimit", c => c.Single(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Piese", "FileUploadHardLimit");
        }
    }
}
