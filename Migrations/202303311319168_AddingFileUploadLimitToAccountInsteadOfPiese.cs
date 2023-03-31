namespace Trippin_Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingFileUploadLimitToAccountInsteadOfPiese : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "FileUploadHardLimit", c => c.Single(nullable: false));
            DropColumn("dbo.Piese", "FileUploadHardLimit");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Piese", "FileUploadHardLimit", c => c.Single(nullable: false));
            DropColumn("dbo.AspNetUsers", "FileUploadHardLimit");
        }
    }
}
