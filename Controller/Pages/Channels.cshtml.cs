using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebMatrix.Data;
using WebMatrix.WebData;

namespace Controller.Pages
{
    public class ChannelsModel : PageModel
    {
        public void OnGet()
        {
        }
        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {

                using (var db = new Model.DeviceChannels())
                {
                    db.Channels.Add(new Model.Channel { ChannelName = ChannelNameIn, Url = URLIn });
                    db.SaveChanges();
                }

                return RedirectToPage("Channels");
            }
            else
            {
                return Page();
            }
        }
        [BindProperty]
        [Required]
        [MinLength(2)]
        public string ChannelNameIn { get; set; }

        [BindProperty]
        [Required]
        [MinLength(6)]
        public string URLIn { get; set; }
    }
}