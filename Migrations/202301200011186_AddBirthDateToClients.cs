namespace Trippin_Website.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddBirthDateToClients : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Clients", "BirthDate", c => c.DateTime(nullable: true));
        }

        public override void Down()
        {
            DropColumn("dbo.Clients", "BirthDate");
        }
    }
}
