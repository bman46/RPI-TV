using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Controller.API
{
    [Route("api/[controller]")]
    public class ValuesController
    {
        // GET: api/<controller>
        
        // GET api/<controller>/5
        [HttpGet("{id}")]
        public string Get(string DeviceName)
        {
            using (var db = new Model.DeviceChannels())
            {
                foreach (var device in db.Devices)
                {
                    if(device.DeviceName == DeviceName)
                    {
                        foreach(var channel in db.Channels)
                        {
                            if(channel.ChannelID == device.SetChannel)
                            {
                                return channel.Url;
                            }
                        }
                    }
                }
            }
            return "NA";
        }

        // POST api/<controller>
        [HttpPost]
        public string Add([FromBody]string Name)
        {
            using (var db = new Model.DeviceChannels())
            {
                if(Name != null && Name != "")
                {
                    db.Devices.Add(new Model.Device { DeviceName = Name });
                    db.SaveChanges();
                }
            }
            return "processed";
        }
        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
