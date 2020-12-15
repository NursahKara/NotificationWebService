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
using System.Data.SqlClient;
namespace NotificationMaker
{
    public partial class Form1 : Form
    {
        private static string con = "Data Source=IFS-NURSAHK\\NURSAHSQLSERVER;Initial Catalog=WebApiNotification;User ID=sa;Password=Qaz2020!";
        private static string host = "http://localhost/NotificationWebService";
        public Form1()
        {
            InitializeComponent();
            using (SqlConnection connection = new SqlConnection(con))
            {
                SqlDataAdapter da=new SqlDataAdapter("SELECT * FROM NotificationSettings WHERE 1 = 0", con);
                DataSet ds = new DataSet();
                connection.Open();
                da.Fill(ds, "NotificationSettings");
                connection.Close();
                int i = 0;
                foreach(var column in ds.Tables[0].Columns)
                {
                    if (i++ < 2)
                        continue;
                    cmbNotificationSettings.Items.Add(column.ToString());
                }
            }
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
                    var category = cmbNotificationSettings.SelectedItem.ToString();
                    var model = new NotificationModel()
                    {
                        Message = message,
                        Title = title,
                        Category = category ?? "DEFAULT",
                        ReceiverUserGuid = "aa9b3fc9-b0da-4440-b9da-2cc29437c91a",
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
