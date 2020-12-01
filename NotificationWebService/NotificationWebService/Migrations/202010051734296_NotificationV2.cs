namespace NotificationWebService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NotificationV2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Notifications", "ReceiverUserGuid", c => c.String(nullable: false));
            DropColumn("dbo.Notifications", "SenderUserGuid");
            DropColumn("dbo.Notifications", "ReceiverGuid");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Notifications", "ReceiverGuid", c => c.String(nullable: false));
            AddColumn("dbo.Notifications", "SenderUserGuid", c => c.String(nullable: false));
            DropColumn("dbo.Notifications", "ReceiverUserGuid");
        }
    }
}
