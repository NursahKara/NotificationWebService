using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace NotificationWebService.Models
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext() : base("databasecnn")
        {

        }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<User> Users { get; set; }
    }
}