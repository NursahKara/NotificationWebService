namespace NotificationWebService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NotificationSettings : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.NotificationSettings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserGuid = c.String(nullable: false),
                        PurchaseReqLine = c.Boolean(nullable: false, defaultValue: true),
                        PurchaseOrder = c.Boolean(nullable: false, defaultValue: true),
                        QuotationLinePart = c.Boolean(nullable: false, defaultValue: true),
                        MaterialRequisition = c.Boolean(nullable: false, defaultValue: true),
                        HizmetBaslik = c.Boolean(nullable: false, defaultValue: true),
                        PurchseOrderMilestoneLine = c.Boolean(nullable: false, defaultValue: true),
                        InvoiceForPayment = c.Boolean(nullable: false, defaultValue: true),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.NotificationSettings");
        }
    }
}
