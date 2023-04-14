namespace Trippin_Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingVersuri : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Versuris",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Title = c.String(),
                        Vers = c.String(),
                        Created = c.DateTime(),
                        Updated = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            AlterColumn("dbo.AspNetUsers", "Versuri", c => c.Guid());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AspNetUsers", "Versuri", c => c.String());
            DropTable("dbo.Versuris");
        }
    }
}
