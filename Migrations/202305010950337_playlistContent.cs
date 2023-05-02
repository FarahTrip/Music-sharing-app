namespace Trippin_Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class playlistContent : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PlaylistContents",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        playlistId = c.String(),
                        piesaId = c.String(),
                        beatId = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            DropColumn("dbo.PlayLists", "piesaId");
            DropColumn("dbo.PlayLists", "beatId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PlayLists", "beatId", c => c.String());
            AddColumn("dbo.PlayLists", "piesaId", c => c.String());
            DropTable("dbo.PlaylistContents");
        }
    }
}
