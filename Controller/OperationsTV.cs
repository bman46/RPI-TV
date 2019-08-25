using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Controller
{
    public class OperationsTV
    {
        //Devices.tv will be for devices and set channel.
        //Channels.tv will be url to channel seperated by line, channel 1 on line 1, etc.
        public static void ChangeChannel(string Name, int Channel)
        {

        }
        public static void CreateChannel(string Url)
        {
            using (System.IO.StreamWriter file =
            new System.IO.StreamWriter("Channels.tv", true))
            {
                file.WriteLine(Url);
            }
        }
        public static void AddDevice(string Name)
        {
            using (System.IO.StreamWriter file =
            new System.IO.StreamWriter("Devices.tv", true))
            {
                file.WriteLine(Name+" 1");
            }
        }
        
    }
}
