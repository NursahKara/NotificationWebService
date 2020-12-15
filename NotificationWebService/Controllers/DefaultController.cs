using DevOne.Security.Cryptography.BCrypt;
using NotificationWebService.Hubs;
using NotificationWebService.Models;
using NotificationWebService.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Web.Http;

namespace NotificationWebService.Controllers
{
    [Authorize]
    public class DefaultController : ApiController
    {
        [AllowAnonymous]
        [HttpPost]
        public IHttpActionResult Register([FromBody] UserRegisterModel model)
        {
            try
            {
                using (var ctx = new DatabaseContext())
                {

                    if (ctx.Users.SingleOrDefault(w => w.UserName.Equals(model.UserName)) != null)
                    {
                        return Json(new
                        {
                            success = false,
                            message = "Kullanıcı adı zaten var"
                        });
                    }
                    var user = new User()
                    {
                        UserName = model.UserName,
                        Password = BCryptHelper.HashPassword(model.Password, BCryptHelper.GenerateSalt(12)),
                        Role = "client",
                        Guid = Guid.NewGuid().ToString(),
                        DateCreated = DateTime.Now,
                        Status = 1
                    };
                    ctx.Users.Add(user);
                    ctx.NotificationSettings.Add(new NotificationSettings()
                    {
                        UserGuid = user.Guid,
                        HizmetBaslik = true,
                        InvoiceForPayment = true,
                        MaterialRequisition = true,
                        PurchaseOrder = true,
                        PurchaseReqLine = true,
                        PurchseOrderMilestoneLine = true,
                        QuotationLinePart = true
                    });
                    ctx.SaveChanges();

                    return Json(new
                    {
                        success = true
                    });
                }
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }

        [HttpPost]
        public IHttpActionResult CreateNotification([FromBody] NotificationModel model)
        {
            try
            {
                using (var ctx = new DatabaseContext())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;
                    ctx.Notifications.Add(new Notification()
                    {
                        Title = model.Title,
                        Message = model.Message,
                        ReceiverUserGuid = model.ReceiverUserGuid,
                        DateCreated = DateTime.Now,
                        Guid = Guid.NewGuid().ToString(),
                        IsReceived = false,
                        IsRead = false,
                        Category = model.Category
                    });
                    ctx.SaveChanges();

                    return Json(new
                    {
                        success = true
                    });
                }
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }

        //[Authorize(Roles = "client")]
        //[HttpGet]
        //public IHttpActionResult GetUserByGuid(string guid)
        //{
        //    try
        //    {
        //        guid = guid.ToLower();
        //        if (String.IsNullOrEmpty(guid))
        //            throw new Exception("Invalid Guid");
        //        var user = NotificationHub.GetUserByGuid(guid);
        //        if (user == null)
        //            throw new Exception("User could not be found");
        //        User model = new User()
        //        {
        //            Guid = user.Guid,
        //            UserName = user.UserName,
        //            Role = user.Role,
        //            DateCreated = user.DateCreated,
        //            Id = user.Id,
        //            Status = user.Status
        //        };
        //        return Json(new
        //        {
        //            user = model,
        //            success = true
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new
        //        {
        //            success = false,
        //            message = ex.Message
        //        });
        //    }
        //}

        [HttpGet]
        public IHttpActionResult GetCurrenUserGuid()
        {
            try
            {
                using (var ctx = new DatabaseContext())
                {
                    var identity = (ClaimsIdentity)User.Identity;
                    var claims = identity.Claims.ToList();
                    var username = claims[0].Value;
                    var denemee = ctx.Users.SingleOrDefault(w => w.UserName == username);
                    return Json(new
                    {
                        success = true,
                        denemee.Guid
                    });
                }
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }

        [NonAction]
        public static bool IsUserExists(string userGuid)
        {
            using (var ctx = new DatabaseContext())
            {
                return ctx.Users.Where(w => w.Guid == userGuid).SingleOrDefault() != null;
            }
        }

        [NonAction]
        public User GetCurrentUser()
        {
            using (var ctx = new DatabaseContext())
            {
                var identity = (ClaimsIdentity)User.Identity;
                var claims = identity.Claims.ToList();
                var username = claims[0].Value;
                return ctx.Users.SingleOrDefault(w => w.UserName == username);
            }
        }

        [NonAction]
        public string GetCurrentUserGuid()
        {
            using (var ctx = new DatabaseContext())
            {
                var identity = (ClaimsIdentity)User.Identity;
                var claims = identity.Claims.ToList();
                var username = claims[0].Value;
                return ctx.Users.SingleOrDefault(w => w.Guid == username).Guid;
            }
        }

        [HttpGet]
        public IHttpActionResult Profile()
        {
            try
            {
                var user = GetCurrentUser();
                var profile = new ProfileModel()
                {
                    UserName = user.UserName,
                    Role = user.Role,
                    Guid = user.Guid
                };
                return Json(new
                {
                    success = true,
                    profile

                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }
        [HttpGet]
        public IHttpActionResult SwitchNotificationSetting(string notificationType)
        {
            try
            {
                var userGuid = GetCurrentUserGuid();
                using (var ctx = new DatabaseContext())
                {
                    var settings = ctx.NotificationSettings.SingleOrDefault(w => w.UserGuid == userGuid);
                    bool value = (bool)settings.GetType().GetProperty(notificationType).GetValue(settings, null);
                    settings.GetType().GetProperty(notificationType).SetValue(settings, !value);
                    ctx.SaveChanges();
                }
                return Json(new
                {
                    success = true
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }
    }
}
