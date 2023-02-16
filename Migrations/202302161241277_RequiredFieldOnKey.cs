namespace Trippin_Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RequiredFieldOnKey : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Piese", "Key", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Piese", "Key", c => c.String());
        }
    }
}
