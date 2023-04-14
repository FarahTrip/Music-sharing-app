namespace Trippin_Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GroupAdminSecond : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Grupuris", "GroupAdminSecondId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Grupuris", "GroupAdminSecondId");
        }
    }
}
