namespace Trippin_Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NoDataBaseGeneratedForPiese : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.Piese");
            AlterColumn("dbo.Piese", "Id", c => c.Guid(nullable: false));
            AddPrimaryKey("dbo.Piese", "Id");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.Piese");
            AlterColumn("dbo.Piese", "Id", c => c.Guid(nullable: false, identity: true));
            AddPrimaryKey("dbo.Piese", "Id");
        }
    }
}
