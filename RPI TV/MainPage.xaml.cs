using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using FFmpegInterop;
using Windows.Media.Core;
using Windows.UI.Popups;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace RPI_TV
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private FFmpegInteropMSS FFmpegMSS;
        public MainPage()
        {
            this.InitializeComponent();

            //stream:
            Settings.Source = "rtmp://192.168.1.51:1935/live/5c33d398e4b02b6251941977_0";
            Stream();
        }
        private void Stream()
        {
            try
            {
                PropertySet options = new PropertySet();
                options.Add("rtsp_flags", "prefer_udp");

                Player.Stop();
                Settings.RefreshSettings();
                FFmpegMSS = FFmpegInteropMSS.CreateFFmpegInteropMSSFromUri(Settings.Source, Settings.AudioDecode, true, options);
                if (FFmpegMSS != null)
                {
                    MediaStreamSource mss = FFmpegMSS.GetMediaStreamSource();
                    if (mss != null)
                    {
                        Player.SetMediaStreamSource(mss);

                    }
                    else
                    {
                        Errors();
                        return;
                    }
                }
                else
                {
                    Errors();
                    return;
                }
            }
            catch
            {
                Errors();
                return;
            }
        }
        private void MediaFailed(object sender, ExceptionRoutedEventArgs e)
        {
            Errors();
        }
        private void Errors()
        {
            Settings.ErrorCount++;
        }
    }
    public class Settings
    {
        public static string Source { get; set; }
        public static string Name { get; set; }
        public static bool AudioDecode { get; set; }
        public static int ErrorCount { get; set; }
        public static void RefreshSettings()
        {

        }
    }
}
