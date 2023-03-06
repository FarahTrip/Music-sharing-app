namespace Trippin_Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PieseFlleSystemModification : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Piese", "PiesaFileNameId", "dbo.PieseFileNames");
            DropIndex("dbo.Piese", new[] { "PiesaFileNameId" });
            AddColumn("dbo.Piese", "BeatId", c => c.Int(nullable: false));
            AddColumn("dbo.Piese", "FileName", c => c.String());
            DropColumn("dbo.Piese", "BeatIDForLinking");
            DropColumn("dbo.Piese", "PiesaFileNameId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Piese", "PiesaFileNameId", c => c.Int());
            AddColumn("dbo.Piese", "BeatIDForLinking", c => c.Int(nullable: false));
            DropColumn("dbo.Piese", "FileName");
            DropColumn("dbo.Piese", "BeatId");
            CreateIndex("dbo.Piese", "PiesaFileNameId");
            AddForeignKey("dbo.Piese", "PiesaFileNameId", "dbo.PieseFileNames", "Id");
        }
    }
}
