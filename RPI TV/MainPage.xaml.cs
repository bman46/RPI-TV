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
using Windows.Storage;
using System.Threading.Tasks;
using System.Net;
using Windows.ApplicationModel.Core;
using Windows.UI.ViewManagement;


// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace RPI_TV
{
    public sealed partial class MainPage : Page
    {
        private FFmpegInteropMSS FFmpegMSS;
        public MainPage()
        {
            this.InitializeComponent();
            //add settings to title bar:
            Window.Current.SizeChanged += Current_SizeChanged_UpdateTitleBar;
            CustomizeTitleBar();
            customTitleBar.Visibility = Visibility.Collapsed;

            //get name of device:
            var deviceInfo = new Windows.Security.ExchangeActiveSyncProvisioning.EasClientDeviceInformation();
            Settings.Name = deviceInfo.FriendlyName.ToString();
            //find server IP:
            ApplicationDataContainer roamingSettings = Windows.Storage.ApplicationData.Current.RoamingSettings;
            Windows.Storage.ApplicationDataCompositeValue composite = (ApplicationDataCompositeValue)roamingSettings.Values["IPConfigs"];
            if (composite != null)
            {
                Settings.ServerIP = composite["ServerIP"] as string;
            }

            //stream event subscriber:
            Settings.SourceChanged += Stream;
            //Worker to check for new source:
            ControllerSync.UpdateLoop();
        }
        public void Stream(Object source, EventArgs e)
        {
            try
            {
                Player.Stop();
                PropertySet options = new PropertySet();
                options.Add("rtsp_flags", "prefer_udp");
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
        private void CustomizeTitleBar()
        {
            // customize title area
            CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = true;
            Window.Current.SetTitleBar(MidLayer);
        }

        private async Task<String> InputTextDialogAsync(string title = "Set Controller IP: ")
        {
            TextBox inputTextBox = new TextBox();
            inputTextBox.AcceptsReturn = false;
            inputTextBox.Height = 32;
            ContentDialog dialog = new ContentDialog();
            dialog.Content = inputTextBox;
            dialog.Title = title + Settings.ServerIP;
            dialog.IsSecondaryButtonEnabled = true;
            dialog.PrimaryButtonText = "Ok";
            dialog.SecondaryButtonText = "Cancel";
            if (await dialog.ShowAsync() == ContentDialogResult.Primary)
            {
                if(inputTextBox.Text == "")
                {
                    return "";
                }
                Settings.ServerIP = inputTextBox.Text;
                ApplicationDataContainer roamingSettings = Windows.Storage.ApplicationData.Current.RoamingSettings;
                Windows.Storage.ApplicationDataCompositeValue composite = new Windows.Storage.ApplicationDataCompositeValue();
                composite["ServerIP"] = inputTextBox.Text;
                roamingSettings.Values["IPConfigs"] = composite;
                return inputTextBox.Text;
            }
            else
            {
                return "";
            }
        }
        private void Current_SizeChanged_UpdateTitleBar(object sender, Windows.UI.Core.WindowSizeChangedEventArgs e)
        {
            ApplicationView view = ApplicationView.GetForCurrentView();
            if (!view.IsFullScreenMode)
            {
                customTitleBar.Visibility = Visibility.Visible;
            } else
            {
                customTitleBar.Visibility = Visibility.Collapsed;
            }
        }

        private void SettingsBTN_Click(object sender, RoutedEventArgs e)
        {
            InputTextDialogAsync();
        }
    }
}
