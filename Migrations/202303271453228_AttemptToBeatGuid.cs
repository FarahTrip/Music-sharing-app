namespace Trippin_Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AttemptToBeatGuid : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.Beats");
            AddPrimaryKey("dbo.Beats", "IdBun");
            DropColumn("dbo.Beats", "Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Beats", "Id", c => c.Int(nullable: false));
            DropPrimaryKey("dbo.Beats");
            AddPrimaryKey("dbo.Beats", "Id");
        }
    }
}
