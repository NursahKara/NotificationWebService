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
            ConsoleKeyInfo keyinfo;
            string host = "https://localhost:44306/";

            Console.Title = "Chat App";
            while (true)
            {
                var hubConnection = new HubConnection(host);
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
                        Console.WriteLine("Connected");

                        notificationHub.On("ReceiveNotification", notification =>
                        {
                            Console.WriteLine(notification);
                        });
                        while (true)
                        {
                            Console.ReadLine();  //entera basınca bildirim düşsün
                            notificationHub.Invoke("SendNotification").ContinueWith(task1 =>
                            {
                                if (task1.IsFaulted)
                                {
                                    Console.WriteLine("There was an error calling send: {0}", task1.Exception.GetBaseException());
                                }
                                else
                                {
                                    //Console.WriteLine(task1.Result);
                                }
                            });
                        }
                    }
                }).Wait();

                Console.Read();
                hubConnection.Stop();
            }

        }
    }
}
