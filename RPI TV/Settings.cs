using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPI_TV
{
    public class Settings
    {
        public static string Source
        {
            get
            {
                return SourceStorage;
            }
            set
            {
                SourceStorage = value;
                SourceChanged(null, EventArgs.Empty);
            }
        }
        private static string SourceStorage { get; set; }
        public static string ServerIP { get; set; }
        public static string Name { get; set; }
        public static bool AudioDecode { get; set; }
        public static int ErrorCount { get; set; }

        //Event:
        public static event EventHandler SourceChanged = delegate {};
    }
}
