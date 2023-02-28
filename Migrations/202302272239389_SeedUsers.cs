namespace Trippin_Website.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class SeedUsers : DbMigration
    {
        public override void Up()
        {
            Sql(@"
INSERT INTO [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName]) VALUES (N'bc60370c-90e4-4c3d-b34f-7565723cb469', N'sorin.mihailescu@outlook.com', 0, N'ALRRWVmClxvTCcf0Jvxk1oequL3YUK+DJQ+N9ygkcHCHxwa89SOl7Gb//3Y6RYJqRw==', N'05e4765b-c588-48d7-88ff-e18f142ab21d', NULL, 0, 0, NULL, 1, 0, N'sorin.mihailescu@outlook.com')
INSERT INTO [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName]) VALUES (N'd30cf49f-f521-449c-91f7-112093735b21', N'Admin@admin', 0, N'AEOU80aREp3J7JmsVHSCgXZCapUMabjFcWDz+xaNcIh2ILtS/MNJXwixs/a7UzkA0g==', N'223aa8f9-af1c-4b65-8e17-00f4a1615d91', NULL, 0, 0, NULL, 1, 0, N'Admin@admin')

INSERT INTO [dbo].[AspNetRoles] ([Id], [Name]) VALUES (N'47db5674-87ba-471d-ad7c-7c4aae7958d8', N'Admin')

INSERT INTO [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'd30cf49f-f521-449c-91f7-112093735b21', N'47db5674-87ba-471d-ad7c-7c4aae7958d8')

");
        }

        public override void Down()
        {
        }
    }
}
