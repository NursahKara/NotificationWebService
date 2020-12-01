using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NotificationWebService.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        public string Role { get; set; }
       
        public string Guid { get; set; }
        public DateTime? DateCreated { get; set; }
        public short? Status { get; set; } //0: passive, 1: active, 2: removed
    }
}