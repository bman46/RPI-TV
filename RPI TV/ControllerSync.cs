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
