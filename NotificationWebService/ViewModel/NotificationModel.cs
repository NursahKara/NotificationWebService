using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NotificationWebService.ViewModel
{
    public class NotificationModel
    {
        public string Guid { get; set; }
        public string ReceiverUserGuid { get; set; }
        public string Message { get; set; }
        public string Title { get; set; }
        public string Category { get; set; }
        public DateTime DateCreated { get; set; }
    }
}