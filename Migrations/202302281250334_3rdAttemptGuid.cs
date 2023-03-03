namespace Trippin_Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _3rdAttemptGuid : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.Piese");
            AddColumn("dbo.Piese", "Id", c => c.Guid(nullable: false, identity: true));
            AddPrimaryKey("dbo.Piese", "Id");
            DropColumn("dbo.Piese", "GuidId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Piese", "GuidId", c => c.Guid(nullable: false));
            DropPrimaryKey("dbo.Piese");
            DropColumn("dbo.Piese", "Id");
            AddPrimaryKey("dbo.Piese", "GuidId");
        }
    }
}
