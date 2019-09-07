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
using System.Diagnostics;


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
            UpdateLoop();
        }
        public void Stream(Object source, EventArgs e)
        {
            try
            {
                PropertySet options = new PropertySet();
                Player.Stop();
                options.Add("rtsp_transport", "tcp");

                FFmpegMSS = FFmpegInteropMSS.CreateFFmpegInteropMSSFromUri(Settings.Source, false, true, options);
                if (FFmpegMSS != null)
                {
                    MediaStreamSource mss = FFmpegMSS.GetMediaStreamSource();
                    if (mss != null)
                    {
                        Player.SetMediaStreamSource(mss);

                    }
                    else
                    {
                        Errors(1);
                        return;
                    }
                }
                else
                {
                    Errors(2);
                    return;
                }
            }
            catch
            {
                Errors(3);
                return;
            }
        }
        public void UpdateLoop()
        {
            Task.Run(async () =>
            {
                while (true) {
                    Windows.Web.Http.HttpClient httpClient = new Windows.Web.Http.HttpClient();
                    Uri RequestUri = new Uri("http://" + Settings.ServerIP + "/api/Values/" + Settings.Name);
                    Windows.Web.Http.HttpResponseMessage httpResponse = new Windows.Web.Http.HttpResponseMessage();
                    try
                    {
                        httpResponse = await httpClient.GetAsync(RequestUri);
                        httpResponse.EnsureSuccessStatusCode();
                        string Response = await httpResponse.Content.ReadAsStringAsync();
                        if (Response == "NaN")
                        {
                            await ControllerSync.AddDevice();
                        }
                        else
                        {
                            if (Settings.Source != Response || Settings.Error == true) 
                            {
                                Settings.Error = false;
                                Debug.WriteLine(Response);
                                await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => {
                                    Settings.Source = Response;
                                });
                            }
                        }
                    }
                    catch
                    {
                        //Do Nothing
                    }
                    await Task.Delay(TimeSpan.FromSeconds(5));
                }
            });
        }
        private void MediaFailed(object sender, ExceptionRoutedEventArgs e)
        {
            Debug.WriteLine(e.ToString());
        }
        private void Errors(int area = 0)
        {
            Debug.WriteLine("Error " + area);
            Settings.Error = true;
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
