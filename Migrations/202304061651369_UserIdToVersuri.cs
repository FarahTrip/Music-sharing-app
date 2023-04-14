namespace Trippin_Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserIdToVersuri : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Versuris", "UserId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Versuris", "UserId");
        }
    }
}
