namespace Trippin_Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingBeatIdForLinkInPiese : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Piese", "BeatId", "dbo.Beats");
            DropIndex("dbo.Piese", new[] { "BeatId" });
            AddColumn("dbo.Piese", "BeatIDForLinking", c => c.Int(nullable: false));
            AlterColumn("dbo.Piese", "Name", c => c.String(nullable: false));
            DropColumn("dbo.Piese", "BeatId");
            DropTable("dbo.Beats");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Beats",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        Key = c.String(),
                        Bpm = c.Int(nullable: false),
                        Style = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Piese", "BeatId", c => c.Int(nullable: false));
            AlterColumn("dbo.Piese", "Name", c => c.String());
            DropColumn("dbo.Piese", "BeatIDForLinking");
            CreateIndex("dbo.Piese", "BeatId");
            AddForeignKey("dbo.Piese", "BeatId", "dbo.Beats", "Id", cascadeDelete: true);
        }
    }
}
