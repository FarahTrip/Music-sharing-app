namespace Trippin_Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeleteDateTime : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Beats", "DateAdded");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Beats", "DateAdded", c => c.DateTime(nullable: false));
        }
    }
}
