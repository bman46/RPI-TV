using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Controller
{
    public class Model
    {
        public class DeviceChannels : DbContext
        {
            public DbSet<Channel> Channels { get; set; }
            public DbSet<Device> Devices { get; set; }
            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                optionsBuilder.UseSqlite("Data Source=Database.db");
            }
        }
        public class Channel
        {
            public int ChannelID { get; set; }
            public string ChannelName { get; set; }
            public string Url { get; set; }
        }
        public class Device
        {
            public int ID { get; set; }
            public string DeviceName { get; set; }
            public int SetChannel { get; set; }
        }
    }
}
