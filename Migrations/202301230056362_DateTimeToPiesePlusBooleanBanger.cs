namespace Trippin_Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DateTimeToPiesePlusBooleanBanger : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Piese", "DateCreated", c => c.DateTime(nullable: false));
            AddColumn("dbo.Piese", "DateModified", c => c.DateTime());
            AddColumn("dbo.Piese", "StyleId", c => c.Byte(nullable: false));
            AddColumn("dbo.Piese", "IsBanger", c => c.Boolean(nullable: false));
            AddColumn("dbo.Piese", "Style_Id", c => c.Int());
            CreateIndex("dbo.Piese", "Style_Id");
            AddForeignKey("dbo.Piese", "Style_Id", "dbo.StyleOfs", "Id");
            DropColumn("dbo.Piese", "Style");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Piese", "Style", c => c.String());
            DropForeignKey("dbo.Piese", "Style_Id", "dbo.StyleOfs");
            DropIndex("dbo.Piese", new[] { "Style_Id" });
            DropColumn("dbo.Piese", "Style_Id");
            DropColumn("dbo.Piese", "IsBanger");
            DropColumn("dbo.Piese", "StyleId");
            DropColumn("dbo.Piese", "DateModified");
            DropColumn("dbo.Piese", "DateCreated");
        }
    }
}
