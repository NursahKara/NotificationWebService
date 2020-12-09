using Microsoft.AspNet.SignalR;
using NotificationWebService.Models;
using NotificationWebService.ViewModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using AuthorizeAttribute = Microsoft.AspNet.SignalR.AuthorizeAttribute;

namespace NotificationWebService.Hubs
{
    [Authorize]
    public class NotificationHub : Hub
    {
        private static IHubContext hubContext = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
        public void MarkNotificationsAsReceived()
        {
            using (var ctx = new DatabaseContext())
            {
                var userGuid = GetCurrentUserGuid();
                var notifications = ctx.Notifications.Where(w => w.ReceiverUserGuid.Equals(userGuid) && w.IsReceived == false).ToList();
                foreach (var item in notifications)
                {
                    item.IsReceived = true;
                    item.DateReceived = DateTime.Now;
                }
                ctx.SaveChanges();
            }
        }
        public void MarkNotificationAsRead(string guid)
        {
            using (var ctx = new DatabaseContext())
            {
                var userGuid = GetCurrentUserGuid();
                var notifications = ctx.Notifications.Where(w => w.Guid == guid.ToLower()).ToList();
                foreach (var item in notifications)
                {
                    item.IsRead = true;
                    item.DateRead = DateTime.Now;
                }
                ctx.SaveChanges();
            }
        }

        public void CreateNotification(NotificationModel model)
        {
            using (var ctx = new DatabaseContext())
            {
                if (ctx.Users.SingleOrDefault(w => w.Guid == model.ReceiverUserGuid.ToLower()) == null)
                {
                    Clients.Caller.AddError("No such user with the given guid");
                    return;
                }
                model.Guid = Guid.NewGuid().ToString();
                model.DateCreated = DateTime.Now;
                var notification = new Notification()
                {
                    Message = model.Message,
                    Title = model.Title,
                    ReceiverUserGuid = model.ReceiverUserGuid.ToLower(),
                    Category = model.Category.ToUpper(new CultureInfo("tr-TR")),
                    DateCreated = model.DateCreated,
                    Guid = model.Guid,
                    IsReceived = false,
                    IsRead = false
                };
                ctx.Notifications.Add(notification);
                ctx.SaveChanges();
                var list = new List<NotificationModel>
                {
                    model
                };
                SendNotifications(list);
            }
        }
        public void SendNotifications(List<NotificationModel> notifications)
        {
            if (notifications.Count() > 0)
            {
                foreach (var notification in notifications)
                {
                    Clients.User(notification.ReceiverUserGuid).ReceiveNotifications(new
                    {
                        notification
                    });
                }
            }
        }
        public void GetUnreceivedNotifications()
        {
            using (var ctx = new DatabaseContext())
            {
                var userGuid = GetCurrentUserGuid();
                var notifications = ctx.Notifications.Where(w => w.ReceiverUserGuid.Equals(userGuid) && w.IsReceived == false).ToList();
                List<NotificationModel> notificationModels = new List<NotificationModel>();
                foreach (var notification in notifications)
                {
                    notificationModels.Add(new NotificationModel()
                    {
                        Guid = notification.Guid,
                        DateCreated = notification.DateCreated,
                        Message = notification.Message,
                        ReceiverUserGuid = notification.ReceiverUserGuid,
                        Title = notification.Title,
                        Category = notification.Category
                    });
                }
                SendNotifications(notificationModels);
            }
        }
        public override Task OnConnected()
        {
            if (!Context.User.Identity.IsAuthenticated)
            {
                Clients.Caller.AddError("You are unauthorized");
            }
            GetUnreceivedNotifications();
            return base.OnConnected();
        }
        private string GetCurrentUserGuid()
        {
            var userGuid = (Context.User.Identity as ClaimsIdentity).Name;
            return userGuid;
        }
    }
}