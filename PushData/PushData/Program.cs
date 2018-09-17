using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Configuration;
using Microsoft.Owin.Hosting;
using PushData.hub;
using PushData.Service;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PushData
{
    class Program
    {

        private static bool IsRun = true;
        private static bool done = false;

        static void Main(string[] args)
        {
            var url = ConfigurationManager.AppSettings["HostUrl"];
            using (WebApp.Start(url))
            {
                Console.WriteLine("Server running on {0}", url);

                var command = "";
                PushData();

                while (!done)
                {
                    command = Console.ReadLine();
                    if (command.ToUpper() == "EXIT") done = true;
                }
                Console.ReadLine();
            }
        }

        static void PushData()
        {
            var task = Task.Factory.StartNew(() =>
            {
                var hub = GlobalHost.ConnectionManager.GetHubContext<HubHelper>();
                
                while (IsRun)
                {
                    Thread.Sleep(1000);
                    long finalBlockNumber;
                    var info = ChainDataService.GetGUCAutodataInfo(out finalBlockNumber);
                    if(info.GetHashCode() == 0 || finalBlockNumber == 0)
                    {
                        continue;
                    }
                    var clients = HubHelper.connections.Where(x => x.Value.LastFinalBlockNumber != finalBlockNumber).Select(x => x.Key).ToList();
                    if (clients.Any() == false)
                    {
                        continue;
                    }
                    hub.Clients.Clients(clients).getMessage(info);
                    foreach(var item in HubHelper.connections)
                    {
                        if (clients.Contains(item.Key))
                        {
                            item.Value.LastFinalBlockNumber = finalBlockNumber;
                        }
                    }
                    Console.Out.Flush();
                }
            });
        }
    }
}
