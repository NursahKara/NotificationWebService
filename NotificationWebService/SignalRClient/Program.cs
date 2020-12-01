using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SignalRClient
{
    class Program
    {
        private static readonly HttpClient Client = new HttpClient();
        public static void Main()
        {
            Run().GetAwaiter().GetResult();
        }
        private static async Task Run()
        {
            string host = "http://localhost/NotificationWebService";

            Console.Title = "Notification App";
            while (true)
            {
                string bearer = "7X15hxSn-_CY38c8qokznO48fMH9vr9nWJo8g4W3HM-Szj3T0LhfiG9vYcZVkfT56UUifFqEY0GWNuoCMrHw0aGDEHhrlveBfra1k3Q00k-jgBRYw5rWTTbHJ5JInBgl0qU-NIQjNEAH3b5-PAAVYkeqoeokAFBBFdEg2-ilI2A19k9-LhGo6Rt1EPcK4JTmKYmAUq0TvyBRSdAdeoIR0cdbkDPupGk2pdoULo7DRpbztp8bvp63ujIM-P7XBJ317VzmT44Ws7Ug5PVPWQbQmXKPUVPRW4XCDdkpVdp4bCPcDpZr8F6TxWcopd4KvrPgSXrD0BnkDIxaXfyK6HxrmQ";
                //string bearer = "MPIn-_t1cBKNFce-x9vWLSuSwzIQemk6IvpaQLfJM8XmaVGRB62osQzrbSEqEZLYVP0dJ6vRJ6MA2IdAZ8-zvzljyr1rhdBRESdnyoBtTkX2NqnqxbXNwgWZ1Esp6TXYPm1vzNzBzgibf-Wuljkv0_6FX7GqrxIU9gwUQUtOoAn9xg72OuZIbQOhjM4TXVsP5F_Fp4UzMe7kr267uyevsRjL4IGuhR2txrA19Tjr7C-4qSRoqqE63Hs944r6GdByAtRzNVkpHm1bOjpYNmufrMB94c7LPDA02hJ4_Y_WnUc3JgVCbQonN4ztBj9RrwRuARvilHS9nnqrKdbaUm-dJw";
                var hubConnection = new HubConnection(host, $"bearer={bearer}");
                IHubProxy notificationHub = hubConnection.CreateHubProxy("NotificationHub");
                hubConnection.Start().ContinueWith(task =>
                {
                    if (task.IsFaulted)
                    {
                        Console.WriteLine("There was an error opening the connection:{0}",
                                          task.Exception.GetBaseException());
                    }
                    else
                    {
                        notificationHub.On("ReceiveNotifications", model =>
                        {
                            Console.WriteLine(model.count + " adet bildiriminiz var");
                            foreach (var notification in model.notifications)
                            {
                                Console.WriteLine(notification.Message);
                                Console.WriteLine(notification.DateCreated);
                            }
                        });
                        Console.WriteLine("Connected");
                        notificationHub.Invoke("GetUnreceivedNotifications");
                        //while (true)
                        //{
                        //    var message = Console.ReadLine();  //entera basınca bildirim düşsün
                        //}
                    }
                }).Wait();

                Console.Read();
                hubConnection.Stop();
            }

        }
    }
}
