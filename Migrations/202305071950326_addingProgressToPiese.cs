namespace Trippin_Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addingProgressToPiese : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Piese", "Currentprogress", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Piese", "Currentprogress");
        }
    }
}
