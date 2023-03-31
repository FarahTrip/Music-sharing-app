namespace Trippin_Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FinilizingGuidBeat : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.Beats");
            AddColumn("dbo.Beats", "IdBun", c => c.Guid(nullable: false, identity: true));
            AlterColumn("dbo.Beats", "Id", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.Beats", "Id");
            DropColumn("dbo.Beats", "asd");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Beats", "asd", c => c.Guid());
            DropPrimaryKey("dbo.Beats");
            AlterColumn("dbo.Beats", "Id", c => c.Int(nullable: false, identity: true));
            DropColumn("dbo.Beats", "IdBun");
            AddPrimaryKey("dbo.Beats", "Id");
        }
    }
}
