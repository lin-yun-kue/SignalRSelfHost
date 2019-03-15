using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;
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
        private static readonly string _url = ConfigurationManager.AppSettings["ApiUrl"];
        private static readonly HttpClient _client = new HttpClient();
        private static readonly Logger _logger = NLog.LogManager.GetCurrentClassLogger();

        static ChainDataService()
        {
            _client.BaseAddress = new Uri(_url);
        }

        public static object GetGUCAutodataInfo(out long finalblocknumber)
        {
            try
            {
                var requestUrl = "autodatainfo?item=bnn";
                var response = _client.GetAsync(requestUrl).Result;
                if (response.IsSuccessStatusCode == false)
                {
                    throw new Exception($"{DateTime.Now}:{_url} fail");
                }
                var resultJSON = response.Content.ReadAsStringAsync().Result;

                dynamic data = JsonConvert.DeserializeObject(resultJSON);
                finalblocknumber = (int)data.finalblocknumber;
                return data;
            }
            catch (Exception e)
            {
                _logger.Error($"call {_url} fail");
                _logger.Error(e.Message);
                Console.WriteLine($"{DateTime.Now}:{e.Message}");
                Console.Out.Flush();
                finalblocknumber = 0;
                return new { };
            }
        }
    }
}
