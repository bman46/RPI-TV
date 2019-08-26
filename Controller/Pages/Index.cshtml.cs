using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Controller.Pages
{
    public class IndexModel : PageModel
    {
        public void OnGet()
        {
            using (var db = new Model.DeviceChannels())
            {
                foreach(var device in db.Devices)
                {
                    DeviceList.Add(new SelectListItem { Value = device.DeviceName, Text = device.DeviceName });
                }
                foreach(var channel in db.Channels)
                {
                    ChannelList.Add(new SelectListItem { Value = channel.ChannelID.ToString(), Text = channel.ChannelName });
                }
            }
        }
        public IActionResult OnPost(string Change, string Delete)
        {
            if (!string.IsNullOrEmpty(Change))
            {
                if (ModelState.IsValid)
                {
                    using (var db = new Model.DeviceChannels())
                    {
                        var device = db.Devices.Single(b => b.DeviceName == SelectedDevice);
                        device.SetChannel = SelectedChannel;
                        db.SaveChanges();
                    }
                }
                return RedirectToPage("Index");
            } else if (!string.IsNullOrEmpty(Delete))
            {
                if (string.IsNullOrEmpty(SelectedDevice))
                {
                    ModelState.AddModelError("SelectedDevice", "You must select a device.");
                    return Page();
                }
                using (var db = new Model.DeviceChannels())
                {
                    var device = db.Devices.Where(d => d.DeviceName.Contains(SelectedDevice));
                    db.Devices.Remove(device.SingleOrDefault());
                    db.SaveChanges();
                }
                return RedirectToPage("Index");
            }
            return Page();
        }
        public List<SelectListItem> DeviceList { get; set; } = new List<SelectListItem> { };
        public List<SelectListItem> ChannelList { get; set; } = new List<SelectListItem> { };
        [BindProperty]
        [Required]
        public string SelectedDevice { get; set; }
        [BindProperty]
        [Required]
        public int SelectedChannel { get; set; }
    }
}
