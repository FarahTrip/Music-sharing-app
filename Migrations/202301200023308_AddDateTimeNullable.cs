namespace Trippin_Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDateTimeNullable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Clients", "BirthDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Clients", "BirthDate", c => c.DateTime(nullable: false));
        }
    }
}
