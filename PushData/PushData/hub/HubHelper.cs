using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using PushData.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PushData.hub
{
    [HubName("PushData")]
    public class HubHelper : Hub
    {
        public static List<string> lstConnectionID = new List<string>();

        //public void Send(ChainData chainData)
        //{
        //    Clients.All.getMessage(chainData.Timestamp, chainData.TransactionNumber);
        //}

        public override Task OnConnected()
        {
            var cid = Context.ConnectionId;
            Console.WriteLine($"Connect:{cid}");
            lstConnectionID.Add(cid);
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            var cid = Context.ConnectionId;
            lstConnectionID.Remove(cid);
            Console.WriteLine($"Disconnect:{cid}");
            return base.OnDisconnected(stopCalled);
        }
    }
}
