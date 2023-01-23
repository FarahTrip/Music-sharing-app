namespace Trippin_Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PiesaFileNamePropertyToPiesa : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Piese", "PiesaFileNameId", c => c.Int());
            CreateIndex("dbo.Piese", "PiesaFileNameId");
            AddForeignKey("dbo.Piese", "PiesaFileNameId", "dbo.PieseFileNames", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Piese", "PiesaFileNameId", "dbo.PieseFileNames");
            DropIndex("dbo.Piese", new[] { "PiesaFileNameId" });
            DropColumn("dbo.Piese", "PiesaFileNameId");
        }
    }
}
