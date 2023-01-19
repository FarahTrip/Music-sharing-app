namespace Trippin_Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedBeaturiToPiese : DbMigration
    {
        public override void Up()
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
            CreateIndex("dbo.Piese", "BeatId");
            AddForeignKey("dbo.Piese", "BeatId", "dbo.Beats", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Piese", "BeatId", "dbo.Beats");
            DropIndex("dbo.Piese", new[] { "BeatId" });
            DropColumn("dbo.Piese", "BeatId");
            DropTable("dbo.Beats");
        }
    }
}
