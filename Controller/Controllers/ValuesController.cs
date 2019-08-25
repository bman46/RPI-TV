using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Controller.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(string name)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] int Operation, string Name, int Channel = 1, string Url = null)
        {
            if (Operation == 1)
            {
                //Change Channel:
                OperationsTV.ChangeChannel(Name, Channel);
            }
            else if(Operation == 2)
            {
                //Add Device
                OperationsTV.AddDevice(Name);
            }
            else if(Operation == 3)
            {
                //Add Channel
                if (Url == null)
                {
                    return;
                }
                OperationsTV.CreateChannel(Url);
            }
            else
            {
                return;
            }
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
