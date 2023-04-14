namespace Trippin_Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class requieredToVersuri : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Versuris", "Title", c => c.String(nullable: false));
            AlterColumn("dbo.Versuris", "Vers", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Versuris", "Vers", c => c.String());
            AlterColumn("dbo.Versuris", "Title", c => c.String());
        }
    }
}
