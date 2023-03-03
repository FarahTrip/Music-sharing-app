namespace Trippin_Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingGuidToPieseInsteadOfIntForID : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.Piese");
            AddColumn("dbo.Piese", "GuidId", c => c.Guid(nullable: false, identity: true));
            AlterColumn("dbo.Piese", "Id", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.Piese", "Id");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.Piese");
            AlterColumn("dbo.Piese", "Id", c => c.Int(nullable: false, identity: true));
            DropColumn("dbo.Piese", "GuidId");
            AddPrimaryKey("dbo.Piese", "Id");
        }
    }
}
