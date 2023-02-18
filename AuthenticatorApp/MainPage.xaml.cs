using AuthenticatorApp.Services.Auth;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Networking.NetworkOperators;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

using AuthenticatorApp.Models;
using AuthenticatorApp.Services.Encryption;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace AuthenticatorApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            this.isWindowsAuthAvailable();
            /*
            AppAuthenticationService.isWindowsAuthAvailable().ContinueWith((data) => {
                errorBox.Text = data.Result.message;
                Debug.WriteLine(data.Result.flag);
            }); */
        }

        private async void isWindowsAuthAvailable() {

            WindowsAuthData windowsAuthData = await AppAuthenticationService.isWindowsAuthAvailable();
            messageBox.Text = windowsAuthData.message;
           
        }

        
        private async void appLogin(object sender, RoutedEventArgs e) {
            WindowsAuthData windowsAuthAvailability = await AppAuthenticationService.isWindowsAuthAvailable();
            if (windowsAuthAvailability.flag)
            {
                WindowsAuthData windowsAuthVerification = await AppAuthenticationService.authenticate();
                if (windowsAuthVerification.flag) {
                    this.Frame.Navigate(typeof(BlankPage1));
                }
                else {
                    messageBox.Text = windowsAuthVerification.message;
                }
            }
            else {
                messageBox.Text = windowsAuthAvailability.message;

            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void exportKeys(object sender, RoutedEventArgs e)
        {
            FileEncryptionService.ExportKeys();
        }
    }
}
