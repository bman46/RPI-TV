using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Controller
{
    public class NameResolver
    {
        public static string IDToName(int ID)
        {
            using (var db = new Model.DeviceChannels())
            {
                try
                {
                    var ThisChannelName = db.Channels.Where(d => d.ChannelID.ToString().Contains(ID.ToString()));
                    return ThisChannelName.SingleOrDefault().ChannelName;
                }
                catch
                {
                    return "Not Assigned.";
                }
            }
        }
        public static string IDToURL(int ID)
        {
            using (var db = new Model.DeviceChannels())
            {
                try
                {
                    var ThisChannel = db.Channels.Where(d => d.ChannelID.ToString().Contains(ID.ToString()));
                    return ThisChannel.SingleOrDefault().Url;
                }
                catch
                {
                    return "NaN";
                }
            }
        }
    }
}
