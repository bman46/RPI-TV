using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.IO;

namespace Controller.Pages
{
    public class SettingsModel : PageModel
    {
        public void OnGet()
        {

        }
        public ActionResult OnPostDownloadDB()
        {
            DateTime thisDay = DateTime.Today;
            if (System.IO.File.Exists("Database.db"))
            {
                return File("~/Database.db", "application/octet-stream", "Database_" + thisDay.ToString("d") + ".db");
            }
            else
            {
                return null;
            }
        }
    }
}