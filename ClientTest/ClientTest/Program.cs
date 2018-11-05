using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //連線SignalR Hub
            //var connection = new HubConnection("http://localhost:44002");
            var connection = new HubConnection("http://125.227.132.127:44003");
            IHubProxy commHub = connection.CreateHubProxy("PushData");

            //宣告function
            commHub.On<object>("getMessage", x => {
                Console.WriteLine(x);

            });

            bool done = false;
            commHub.On("Exit", () => { done = true; });

            connection.Start()
            .ContinueWith(task =>
            {
                Console.WriteLine("Start");
                //if (!task.IsFaulted)
                //    //default connect to group A
                //    commHub.Invoke("send", "A", DateTime.Now.ToString());
                //else
                //    done = true;
            });

            string command = "";
            while (!done)
            {
                command = Console.ReadLine();
                if (command.ToUpper() == "EXIT") done = true;
            }

            connection.Stop();
        }
    }
}
