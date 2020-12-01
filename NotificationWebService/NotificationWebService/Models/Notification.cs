using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NotificationWebService.Models
{
    public class Notification
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Guid { get; set; }
        [Required]
        public string ReceiverUserGuid { get; set; }
        [Required]
        public string Message { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Category { get; set; }
        public bool IsReceived { get; set; }
        public bool IsRead { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateReceived { get; set; }
        public DateTime? DateRead { get; set; }
    }
}