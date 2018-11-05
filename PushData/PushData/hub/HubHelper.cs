using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using NLog;
using PushData.Model;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PushData.hub
{
    [HubName("PushData")]
    public class HubHelper : Hub
    {
        public static ConcurrentDictionary<string, ConnectionStatus> connections = new ConcurrentDictionary<string, ConnectionStatus>();
        private static readonly Logger _logger = NLog.LogManager.GetCurrentClassLogger();

        //public void Send(ChainData chainData)
        //{
        //    Clients.All.getMessage(chainData.Timestamp, chainData.TransactionNumber);
        //}

        public override Task OnConnected()
        {
            var cid = Context.ConnectionId;
            connections.TryAdd(cid, new ConnectionStatus { LastFinalBlockNumber = 0 });

            _logger.Info($"Connect {DateTime.Now.ToString()} :{cid}");
            _logger.Info($"NumberOfConnection:{connections.Count}");

            Console.WriteLine($"Connect {DateTime.Now.ToString()} :{cid}");
            Console.WriteLine($"NumberOfConnection:{connections.Count}");
            Console.Out.Flush();

            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            var cid = Context.ConnectionId;
            ConnectionStatus value;
            connections.TryRemove(cid, out value);

            _logger.Info($"Disconnect {DateTime.Now.ToString()} :{cid}");
            _logger.Info($"NumberOfConnection:{connections.Count}");

            Console.WriteLine($"Disconnect {DateTime.Now.ToString()} :{cid}");
            Console.WriteLine($"NumberOfConnection:{connections.Count}");
            Console.Out.Flush();

            return base.OnDisconnected(stopCalled);
        }
    }

    public class ConnectionStatus
    {
        public long LastFinalBlockNumber { get; set; }
    }
}
