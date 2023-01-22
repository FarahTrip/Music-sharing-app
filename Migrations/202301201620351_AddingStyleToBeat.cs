namespace Trippin_Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingStyleToBeat : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Beats", "StyleId", c => c.Byte(nullable: false));
            AddColumn("dbo.Beats", "Style_Id", c => c.Int());
            CreateIndex("dbo.Beats", "Style_Id");
            AddForeignKey("dbo.Beats", "Style_Id", "dbo.StyleOfs", "Id");
            DropColumn("dbo.Beats", "Style");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Beats", "Style", c => c.String());
            DropForeignKey("dbo.Beats", "Style_Id", "dbo.StyleOfs");
            DropIndex("dbo.Beats", new[] { "Style_Id" });
            DropColumn("dbo.Beats", "Style_Id");
            DropColumn("dbo.Beats", "StyleId");
        }
    }
}
