using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NotificationWebService.Models
{
    public class NotificationSettings
    {
        public int Id { get; set; }
        [Required]
        public string UserGuid { get; set; }
        public bool PurchaseReqLine { get; set; }
        public bool PurchaseOrder { get; set; }
        public bool QuotationLinePart { get; set; }
        public bool MaterialRequisition { get; set; }
        public bool HizmetBaslik { get; set; }
        public bool PurchseOrderMilestoneLine { get; set; }
        public bool InvoiceForPayment { get; set; }
    }
}