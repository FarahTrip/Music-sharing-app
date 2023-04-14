namespace Trippin_Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingGrupuriAndGropumembers : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Grupuris",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        GroupAdminId = c.String(),
                        Name = c.String(),
                        Created = c.DateTime(),
                        TargetPiesePeLuna = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.GrupuriMembriis",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        GroupId = c.String(),
                        MemberId = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.GrupuriMembriis");
            DropTable("dbo.Grupuris");
        }
    }
}
