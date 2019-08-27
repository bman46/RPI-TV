using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;

namespace RPI_TV
{
    class ControllerSync
    {
        public static void UpdateLoop()
        {
            Task.Run(async () =>
            {
                while (true)
                {
                    Windows.Web.Http.HttpClient httpClient = new Windows.Web.Http.HttpClient();
                    Uri RequestUri = new Uri("http://" + Settings.ServerIP + "/api/Values/" + Settings.Name);
                    Windows.Web.Http.HttpResponseMessage httpResponse = new Windows.Web.Http.HttpResponseMessage();
                    string Response;
                    try
                    {
                        httpResponse = await httpClient.GetAsync(RequestUri);
                        httpResponse.EnsureSuccessStatusCode();
                        Response = await httpResponse.Content.ReadAsStringAsync();
                        if (Response == "NaN")
                        {
                            await AddDevice();
                        }
                        else
                        {
                            if (Settings.Source != Response)
                            { 
                                Settings.Source = Response;
                            }
                        }
                    }
                    catch
                    {
                        //Do Nothing
                    }
                    await Task.Delay(TimeSpan.FromSeconds(5));
                }
            });
        }
        public static async Task AddDevice()
        {
            Debug.WriteLine("New Device");

            HttpClient client = new HttpClient();
            var content = new StringContent("\""+Settings.Name+"\"", Encoding.UTF8, "application/json");
            var result = await client.PostAsync("http://" + Settings.ServerIP + "/api/Values", content);

            Debug.WriteLine("result: "+result);
        }
    }
}
