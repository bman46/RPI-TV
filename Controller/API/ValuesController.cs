﻿using System;
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
        
        // GET api/<controller>/5
        [HttpGet("{DeviceName}")]
        public ActionResult<string> TVChannel(string DeviceName)
        {
            try
            {
                using (var db = new Model.DeviceChannels())
                {
                    //DeviceJSON returnVals = new DeviceJSON();
                    var device = db.Devices.Where(d => d.DeviceName.Contains(DeviceName));
                    if (device.SingleOrDefault().SetChannel == 0)
                    {
                        return "Not Set";
                    }
                    //old:
                    int SelectChannel = device.SingleOrDefault().SetChannel;
                    return NameResolver.IDToURL(SelectChannel);
                }
            }
            catch
            {
                return "NaN";
            }
        }

        // POST api/<controller>
        [HttpPost]
        public string Add([FromBody]string Name)
        {
            using (var db = new Model.DeviceChannels())
            {
                if(Name != null && Name != "")
                {
                    db.Devices.Add(new Model.Device { DeviceName = Name, SetChannel = 0});
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
