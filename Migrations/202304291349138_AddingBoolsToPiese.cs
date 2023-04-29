namespace Trippin_Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingBoolsToPiese : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Piese", "IsPublic", c => c.Boolean(nullable: false));
            AddColumn("dbo.Piese", "IsJustForMyGroup", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Piese", "IsJustForMyGroup");
            DropColumn("dbo.Piese", "IsPublic");
        }
    }
}
