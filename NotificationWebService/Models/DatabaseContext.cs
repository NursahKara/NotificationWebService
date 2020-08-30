using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace NotificationWebService.Models
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext() : base("dbcnn")
        {

        }
        public DbSet<Notifications> Notifications { get; set; }
    }
}