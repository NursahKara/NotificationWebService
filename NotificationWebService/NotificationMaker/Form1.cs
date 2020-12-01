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
            var hubConnection = new HubConnection(host);
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
                    var receiver = txtReceiver.Text;
                    var model = new MessageModel()
                    {
                        Message = title,
                        Title = message,
                        SenderUserGuid = "senderuser",
                        ReceiverUserGuid = receiver,
                        DateCreated = DateTime.Now,
                    };
                    notificationHub.Invoke("CreateNotification",model);
                }
            }).Wait();
            lblStatus.ForeColor = Color.Lime;
            lblStatus.Text = "Sent";
            lblStatus.Visible = true;
            txtMessage.Clear();
            txtTitle.Clear();
            hubConnection.Stop();
        }
    }
}
