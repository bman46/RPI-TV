using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

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
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://"+Settings.ServerIP+ "/api/Values");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(await httpWebRequest.GetRequestStreamAsync()))
            {
                streamWriter.Write(Settings.Name);
            }

        }
    }
}
