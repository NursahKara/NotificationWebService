namespace NotificationWebService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fff : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Users", "Guid", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Users", "Guid", c => c.String(nullable: false));
        }
    }
}
