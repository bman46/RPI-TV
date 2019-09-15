using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//commands to build db:
//dotnet ef migrations add InitialCreate
//dotnet ef database update

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
            public string Protocol { get; set; }
        }
        public class Device
        {
            public int ID { get; set; }
            public string DeviceName { get; set; }
            public int SetChannel { get; set; }
            public int Volume { get; set; }
        }
    }
}
