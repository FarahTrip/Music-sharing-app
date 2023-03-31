namespace Trippin_Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingQuotaAndVersuriToUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Quota", c => c.Single(nullable: false));
            AddColumn("dbo.AspNetUsers", "Versuri", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "Versuri");
            DropColumn("dbo.AspNetUsers", "Quota");
        }
    }
}
