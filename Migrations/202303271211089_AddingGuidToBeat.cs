namespace Trippin_Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingGuidToBeat : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Beats", "asd", c => c.Guid());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Beats", "asd");
        }
    }
}
