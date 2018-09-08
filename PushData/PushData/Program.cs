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

        static void Main(string[] args)
        {
            var url = ConfigurationManager.AppSettings["HostUrl"];
            using (WebApp.Start(url))
            {
                Console.WriteLine("Server running on {0}", url);

                var command = "";
                PushData();
                command = Console.ReadLine();
                if (command.ToUpper() == "EXIT")
                {
                    IsRun = false;
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
                    //todo:get latest block id
                    var id = ChainDataService.GetLatestBlockID();
                    var chainData = ChainDataService.GetBlockByID(100);
                    hub.Clients.All.getMessage(chainData.Timestamp, chainData.TransactionNumber);
                }
            });
        }
    }
}
