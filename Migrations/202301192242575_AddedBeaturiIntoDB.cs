namespace Trippin_Website.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddedBeaturiIntoDB : DbMigration
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

        }

        public override void Down()
        {
            DropTable("dbo.Beats");
        }
    }
}
