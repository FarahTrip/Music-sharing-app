namespace Trippin_Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BeaturiValidationTags : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Beats", "Key", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Beats", "Key", c => c.String());
        }
    }
}
