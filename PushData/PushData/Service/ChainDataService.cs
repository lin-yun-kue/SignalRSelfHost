using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PushData.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PushData.Service
{
    public class ChainDataService
    {
        private static string url = ConfigurationManager.AppSettings["ApiUrl"];

        public static ChainData GetBlockByID(int id)
        {
            try
            {
                var client = new HttpClient();
                var requestUrl = $"{url}/getBlockbyID/{id.ToString()}";
                var response = client.GetAsync(requestUrl).Result;
                if (response.IsSuccessStatusCode == false)
                {
                    return default(ChainData);
                }

                var resultJSON = response.Content.ReadAsStringAsync().Result;
                client.Dispose();

                dynamic data = JObject.Parse(resultJSON);
                var timeStamp = (int)data.result.timestamp;
                var transactionNumber = data.result.transaction.Count;
                var chainData = new ChainData
                {
                    Timestamp = timeStamp,
                    TransactionNumber = transactionNumber
                };
                return chainData;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return default(ChainData);
        }

        public static int GetLatestBlockID()
        {
            //todo: wait api
            return 100;
        }
    }
}
