using Microsoft.AspNet.SignalR;
using NotificationWebService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace NotificationWebService.Hubs
{
    public class NotificationHub : Hub
    {
        public void DeleteNotifications()
        {
            using (var ctx = new DatabaseContext())
            {
                ctx.Notifications.RemoveRange(ctx.Notifications);
                ctx.SaveChanges();
            }
        }
        public void CreateNotification(string title, string message)
        {
            using (var ctx = new DatabaseContext())
            {
                ctx.Notifications.Add(new Notification()
                {
                    Message = message,
                    Title = title
                });
                ctx.SaveChanges();
                foreach (var notification in ctx.Notifications.ToList())
                {
                    Clients.Caller.ReceiveNotification(new
                    {
                        title = notification.Title,
                        message = notification.Message
                    });
                }
            }
        }
        public override Task OnConnected()
        {
            using (var ctx = new DatabaseContext())
            {
                if (ctx.Notifications.Count() > 0)
                {
                    foreach (var notification in ctx.Notifications.ToList())
                    {
                        Clients.All.ReceiveNotification(new
                        {
                            title = notification.Title,
                            message = notification.Message
                        });
                    }
                }
            }
            return base.OnConnected();
        }
    }
}