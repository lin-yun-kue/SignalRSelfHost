using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
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

        //public void Send(ChainData chainData)
        //{
        //    Clients.All.getMessage(chainData.Timestamp, chainData.TransactionNumber);
        //}

        public override Task OnConnected()
        {
            var cid = Context.ConnectionId;
            Console.WriteLine($"Connect:{cid}");
            connections.TryAdd(cid, new ConnectionStatus { LastFinalBlockNumber = 0 });
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            var cid = Context.ConnectionId;
            ConnectionStatus value;
            connections.TryRemove(cid, out value);
            Console.WriteLine($"Disconnect:{cid}");
            return base.OnDisconnected(stopCalled);
        }
    }

    public class ConnectionStatus
    {
        public long LastFinalBlockNumber { get; set; }
    }
}
