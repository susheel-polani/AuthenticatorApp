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
            DataAccess.AddData("facebook.com", "username1", "key1");
            DataAccess.AddData("google.com", "username2", "key2");
            DataAccess.AddData("linkedin.com", "username3", "key3");
        }
        private void getUsernames(object sender, RoutedEventArgs e)
        {

            List<String> rows = DataAccess.GetData("google.com", "username2");
            String usernames="";
            foreach (String row in rows)
            {
                usernames = usernames + row + "\n";
            }
            usernamesTextBox.Text = usernames;
        }

        private void writeFile(object sender, RoutedEventArgs e)
        {
            FileSystemAccess.WriteFile();
        }
    }
}
