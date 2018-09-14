﻿using Newtonsoft.Json;
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

        public static object GetGUCAutodataInfo(out long finalblocknumber)
        {
            var client = new HttpClient();

            try
            {
                var requestUrl = $"{url}/gucautodatainfo";
                var response = client.GetAsync(requestUrl).Result;
                if (response.IsSuccessStatusCode == false)
                {
                    throw new Exception($"{DateTime.Now}:api fail");
                }
                var resultJSON = response.Content.ReadAsStringAsync().Result;
                client.Dispose();

                dynamic data = JsonConvert.DeserializeObject(resultJSON);
                finalblocknumber = (int)data.finalblocknumber;
                return data;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                finalblocknumber = 0;
                return new { };
            }
            finally
            {
                client.Dispose();
            }
        }
    }
}
