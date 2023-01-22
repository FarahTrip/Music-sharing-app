namespace Trippin_Website.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddingDateTimePropsToBeats : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Beats", "Created", c => c.DateTime());
            AddColumn("dbo.Beats", "Modified", c => c.DateTime());
        }

        public override void Down()
        {
            DropColumn("dbo.Beats", "Modified");
            DropColumn("dbo.Beats", "Created");
        }
    }
}
