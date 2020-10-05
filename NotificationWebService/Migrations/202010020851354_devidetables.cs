namespace NotificationWebService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class devidetables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserName = c.String(nullable: false),
                        Password = c.String(nullable: false),
                        Role = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            DropColumn("dbo.Notifications", "UserName");
            DropColumn("dbo.Notifications", "Password");
            DropColumn("dbo.Notifications", "Role");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Notifications", "Role", c => c.String(nullable: false));
            AddColumn("dbo.Notifications", "Password", c => c.String(nullable: false));
            AddColumn("dbo.Notifications", "UserName", c => c.String(nullable: false));
            DropTable("dbo.Users");
        }
    }
}
