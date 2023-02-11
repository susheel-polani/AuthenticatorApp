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
        }
        private async void TriggerWindowsAuthentication(object sender, RoutedEventArgs e)
        {
            string returnMessage = "";

            try
            {
                // Check the availability of fingerprint authentication.
                var ucvAvailability = await Windows.Security.Credentials.UI.UserConsentVerifier.CheckAvailabilityAsync();
                Debug.WriteLine(ucvAvailability);

                switch (ucvAvailability)
                {
                    case Windows.Security.Credentials.UI.UserConsentVerifierAvailability.Available:
                        returnMessage = "Fingerprint verification is available.";
                        break;
                    case Windows.Security.Credentials.UI.UserConsentVerifierAvailability.DeviceBusy:
                        returnMessage = "Biometric device is busy.";
                        break;
                    case Windows.Security.Credentials.UI.UserConsentVerifierAvailability.DeviceNotPresent:
                        returnMessage = "No biometric device found.";
                        break;
                    case Windows.Security.Credentials.UI.UserConsentVerifierAvailability.DisabledByPolicy:
                        returnMessage = "Biometric verification is disabled by policy.";
                        break;
                    case Windows.Security.Credentials.UI.UserConsentVerifierAvailability.NotConfiguredForUser:
                        returnMessage = "The user has no fingerprints registered. Please add a fingerprint to the " +
                                        "fingerprint database and try again.";
                        break;
                    default:
                        returnMessage = "Fingerprints verification is currently unavailable.";
                        break;
                }
            }
            catch (Exception ex)
            {
                returnMessage = "Fingerprint authentication availability check failed: " + ex.ToString();
            }


            if(returnMessage == "Fingerprint verification is available.")
            {
                string returnVerification;

                string userMessage = "Authentication needed to access the Domain specific Key pairs.";

                try
                {
                    // Request the logged on user's consent via fingerprint swipe.
                    var consentResult = await Windows.Security.Credentials.UI.UserConsentVerifier.RequestVerificationAsync(userMessage);

                    switch (consentResult)
                    {
                        case Windows.Security.Credentials.UI.UserConsentVerificationResult.Verified:
                            returnVerification = "Fingerprint verified.";
                            break;
                        case Windows.Security.Credentials.UI.UserConsentVerificationResult.DeviceBusy:
                            returnVerification = "Biometric device is busy.";
                            break;
                        case Windows.Security.Credentials.UI.UserConsentVerificationResult.DeviceNotPresent:
                            returnVerification = "No biometric device found.";
                            break;
                        case Windows.Security.Credentials.UI.UserConsentVerificationResult.DisabledByPolicy:
                            returnVerification = "Biometric verification is disabled by policy.";
                            break;
                        case Windows.Security.Credentials.UI.UserConsentVerificationResult.NotConfiguredForUser:
                            returnVerification = "The user has no fingerprints registered. Please add a fingerprint to the " +
                                            "fingerprint database and try again.";
                            break;
                        case Windows.Security.Credentials.UI.UserConsentVerificationResult.RetriesExhausted:
                            returnVerification = "There have been too many failed attempts. Fingerprint authentication canceled.";
                            break;
                        case Windows.Security.Credentials.UI.UserConsentVerificationResult.Canceled:
                            returnVerification = "Fingerprint authentication canceled.";
                            break;
                        default:
                            returnVerification = "Fingerprint authentication is currently unavailable.";
                            break;
                    }
                }
                catch (Exception ex)
                {
                    returnVerification = "Fingerprint authentication failed: " + ex.ToString();
                }

                if(returnVerification == "Fingerprint verified.")
                {
                    this.Frame.Navigate(typeof(BlankPage1));
                }
                else
                {
                    errorBox.Text = "Authentication failed! Try Again!";
                    errorBox.Visibility = Visibility.Visible;
                }
            }
            else
            {
                errorBox.Text = "Windows Log on Featured Missing/Malfunctioned!";
                errorBox.Visibility = Visibility.Visible;
            }
        }


        private async void TestAuthentication(object sender, RoutedEventArgs e)
        {
            string returnVerification;

            string userMessage = "Authentication needed to access the Domain specific Key pairs.";

            try
            {
                // Request the logged on user's consent via fingerprint swipe.
                var consentResult = await Windows.Security.Credentials.UI.UserConsentVerifier.RequestVerificationAsync(userMessage);
                Debug.WriteLine(consentResult);
                switch (consentResult)
                {
                    case Windows.Security.Credentials.UI.UserConsentVerificationResult.Verified:
                        returnVerification = "Fingerprint verified.";
                        break;
                    case Windows.Security.Credentials.UI.UserConsentVerificationResult.DeviceBusy:
                        returnVerification = "Biometric device is busy.";
                        break;
                    case Windows.Security.Credentials.UI.UserConsentVerificationResult.DeviceNotPresent:
                        returnVerification = "No biometric device found.";
                        break;
                    case Windows.Security.Credentials.UI.UserConsentVerificationResult.DisabledByPolicy:
                        returnVerification = "Biometric verification is disabled by policy.";
                        break;
                    case Windows.Security.Credentials.UI.UserConsentVerificationResult.NotConfiguredForUser:
                        returnVerification = "The user has no fingerprints registered. Please add a fingerprint to the " +
                                        "fingerprint database and try again.";
                        break;
                    case Windows.Security.Credentials.UI.UserConsentVerificationResult.RetriesExhausted:
                        returnVerification = "There have been too many failed attempts. Fingerprint authentication canceled.";
                        break;
                    case Windows.Security.Credentials.UI.UserConsentVerificationResult.Canceled:
                        returnVerification = "Fingerprint authentication canceled.";
                        break;
                    default:
                        returnVerification = "Fingerprint authentication is currently unavailable.";
                        break;
                }
            }
            catch (Exception ex)
            {
                returnVerification = "Fingerprint authentication failed: " + ex.ToString();
            }

            if (returnVerification == "Fingerprint verified.")
            {
                this.Frame.Navigate(typeof(BlankPage1));
            }
            else
            {
                errorBox.Text = "Authentication failed! Try Again!";
                errorBox.Visibility = Visibility.Visible;
            }
        }


        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
