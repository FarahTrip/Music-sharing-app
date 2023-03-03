namespace Trippin_Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _2ndAttemptToAddGuid : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.Piese");
            AlterColumn("dbo.Piese", "GuidId", c => c.Guid(nullable: false));
            AddPrimaryKey("dbo.Piese", "GuidId");
            DropColumn("dbo.Piese", "Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Piese", "Id", c => c.Int(nullable: false));
            DropPrimaryKey("dbo.Piese");
            AlterColumn("dbo.Piese", "GuidId", c => c.Guid(nullable: false, identity: true));
            AddPrimaryKey("dbo.Piese", "Id");
        }
    }
}
