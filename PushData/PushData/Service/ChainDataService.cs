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
        private static readonly HttpClient client = new HttpClient();

        static ChainDataService()
        {
            client.BaseAddress = new Uri(url);
        }

        public static object GetGUCAutodataInfo(out long finalblocknumber)
        {
            

            try
            {
                var requestUrl = "cicautodatainfo";
                var response = client.GetAsync(requestUrl).Result;
                if (response.IsSuccessStatusCode == false)
                {
                    throw new Exception($"{DateTime.Now}:api fail");
                }
                var resultJSON = response.Content.ReadAsStringAsync().Result;

                dynamic data = JsonConvert.DeserializeObject(resultJSON);
                finalblocknumber = (int)data.finalblocknumber;
                return data;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.Out.Flush();
                finalblocknumber = 0;
                return new { };
            }
        }
    }
}
