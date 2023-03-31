namespace Trippin_Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AdddingFileSizeToPiese : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Piese", "FileSize", c => c.Single(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Piese", "FileSize");
        }
    }
}
