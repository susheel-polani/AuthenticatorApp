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
using Microsoft.Data.Sqlite;
using AuthenticatorApp.Services.Encryption;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace AuthenticatorApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BlankPage1 : Page
    {
        public BlankPage1()
        {
            this.InitializeComponent();
        }
        private void getUsernames(object sender, RoutedEventArgs e)
        {
            DataAccess.AddData("susheel98");
            DataAccess.AddData("passwordless-team");
            DataAccess.AddData("thegamer");

            List<String> rows = DataAccess.GetData();
            String usernames="";
            foreach (String row in rows)
            {
                usernames = usernames + row + "\n";
            }
            usernamesTextBox.Text = usernames;
        }

        private void encDec(object sender, RoutedEventArgs e) 
        {
            FileEncryptionService.ExportKeys();
        }
    }
}
