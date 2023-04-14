namespace Trippin_Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingWhoProducedAndMadeTheSong : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.WhoIsOnTheSongs",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        PiesaId = c.String(),
                        ArtistId = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.WhoProducedTheSongs",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        PiesaId = c.String(),
                        ArtistId = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.WhoProducedTheSongs");
            DropTable("dbo.WhoIsOnTheSongs");
        }
    }
}
