namespace NotificationWebService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NotificationSettings_1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.NotificationSettings", "PurchaseReqLine", c => c.Boolean(nullable: false));
            AlterColumn("dbo.NotificationSettings", "PurchaseOrder", c => c.Boolean(nullable: false));
            AlterColumn("dbo.NotificationSettings", "QuotationLinePart", c => c.Boolean(nullable: false));
            AlterColumn("dbo.NotificationSettings", "MaterialRequisition", c => c.Boolean(nullable: false));
            AlterColumn("dbo.NotificationSettings", "HizmetBaslik", c => c.Boolean(nullable: false));
            AlterColumn("dbo.NotificationSettings", "PurchseOrderMilestoneLine", c => c.Boolean(nullable: false));
            AlterColumn("dbo.NotificationSettings", "InvoiceForPayment", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.NotificationSettings", "InvoiceForPayment", c => c.Boolean(nullable: false));
            AlterColumn("dbo.NotificationSettings", "PurchseOrderMilestoneLine", c => c.Boolean(nullable: false));
            AlterColumn("dbo.NotificationSettings", "HizmetBaslik", c => c.Boolean(nullable: false));
            AlterColumn("dbo.NotificationSettings", "MaterialRequisition", c => c.Boolean(nullable: false));
            AlterColumn("dbo.NotificationSettings", "QuotationLinePart", c => c.Boolean(nullable: false));
            AlterColumn("dbo.NotificationSettings", "PurchaseOrder", c => c.Boolean(nullable: false));
            AlterColumn("dbo.NotificationSettings", "PurchaseReqLine", c => c.Boolean(nullable: false));
        }
    }
}
