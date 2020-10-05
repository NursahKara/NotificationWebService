namespace NotificationWebService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class aa : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Notifications", "Guid", c => c.String(nullable: false));
            AddColumn("dbo.Notifications", "SenderUserGuid", c => c.String(nullable: false));
            AddColumn("dbo.Notifications", "ReceiverGuid", c => c.String(nullable: false));
            AddColumn("dbo.Notifications", "Category", c => c.String(nullable: false));
            AddColumn("dbo.Notifications", "IsReceived", c => c.Boolean(nullable: false));
            AddColumn("dbo.Notifications", "IsRead", c => c.Boolean(nullable: false));
            AddColumn("dbo.Notifications", "DateCreated", c => c.DateTime(nullable: false));
            AddColumn("dbo.Notifications", "DateReceived", c => c.DateTime());
            AddColumn("dbo.Notifications", "DateRead", c => c.DateTime());
            AddColumn("dbo.Users", "Guid", c => c.String(nullable: false));
            AddColumn("dbo.Users", "DateCreated", c => c.DateTime(nullable: false));
            AddColumn("dbo.Users", "Status", c => c.Short(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "Status");
            DropColumn("dbo.Users", "DateCreated");
            DropColumn("dbo.Users", "Guid");
            DropColumn("dbo.Notifications", "DateRead");
            DropColumn("dbo.Notifications", "DateReceived");
            DropColumn("dbo.Notifications", "DateCreated");
            DropColumn("dbo.Notifications", "IsRead");
            DropColumn("dbo.Notifications", "IsReceived");
            DropColumn("dbo.Notifications", "Category");
            DropColumn("dbo.Notifications", "ReceiverGuid");
            DropColumn("dbo.Notifications", "SenderUserGuid");
            DropColumn("dbo.Notifications", "Guid");
        }
    }
}
