using Microsoft.AspNet.SignalR.Client;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NotificationWebService.Models;
using NotificationWebService.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace NotificationMaker
{
    public partial class Form1 : Form
    {
        private static string host = "http://localhost/NotificationWebService";
        public Form1()
        {
            InitializeComponent();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            var token = "6UmQSkZ_J5e1uizIW8wBqMZgcmZ_KvVB7fnZwZXf02pl-rq1M3yEGWYGRuqrfD5NWgacH-1E90GXNVqmBvQxBg8Pku5jsX_Fj9X2x-YuUbi9nqHV3Lpycrw4OxoQ3iPUe0QBLCD0KtDMxHkEUqNhBR7w24GU97MeUDN_wryTry_12FjovciSV1iSzifOhstYXK6CmBermHV6MpHoontDJFKWZ_2pjYF0hMBKJ8VkIr9y0BRPcXd__okaJ_eCKCFa2TAPwnOfOXmCALX4pIk4DaTMyyEnTRUq7qmK4AB1eiw8EwYMifPVYMTTiGMmahy5uaUS9N7O7cc37gsK2c-UuA";
            var hubConnection = new HubConnection(host, $"bearer={token}");
            IHubProxy notificationHub = hubConnection.CreateHubProxy("NotificationHub");
            lblStatus.Text = "Waiting";
            lblStatus.Visible = true;
            hubConnection.Start().ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    task.Exception.GetBaseException();
                }
                else
                {
                    var title = txtTitle.Text;
                    var message = txtMessage.Text;
                    var category = txtCategory.Text;
                    var model = new NotificationModel()
                    {
                        Message = message,
                        Title = title,
                        Category = category ?? "DEFAULT",
                        ReceiverUserGuid = "a61c2f26-c603-46c7-a170-4ae9ac969e18",
                    };
                    notificationHub.Invoke("CreateNotification", model);
                }
            }).Wait();
            lblStatus.ForeColor = Color.Lime;
            lblStatus.Text = "Sent";
            lblStatus.Visible = true;
            txtMessage.Clear();
            txtTitle.Clear();
            hubConnection.Stop();
        }
        public class NotificationModel
        {
            public string Title { get; set; }
            public string Message { get; set; }
            public string Category { get; set; }
            public string ReceiverUserGuid { get; set; }
        }
    }
}
