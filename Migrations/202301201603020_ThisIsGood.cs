namespace Trippin_Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ThisIsGood : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.StyleOfs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AlterColumn("dbo.Beats", "Name", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Beats", "Name", c => c.String());
            DropTable("dbo.StyleOfs");
        }
    }
}
