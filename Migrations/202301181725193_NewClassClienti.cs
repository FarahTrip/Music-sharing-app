namespace Trippin_Website.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class NewClassClienti : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Clients",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Name = c.String(),
                    IsSubscribed = c.Boolean(nullable: false),
                })
                .PrimaryKey(t => t.Id);

        }

        public override void Down()
        {
            DropTable("dbo.Clients");
        }

    }
}
