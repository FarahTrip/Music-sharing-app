namespace Trippin_Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DateCreatedNotNullable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Beats", "Created", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Beats", "Created", c => c.DateTime());
        }
    }
}
