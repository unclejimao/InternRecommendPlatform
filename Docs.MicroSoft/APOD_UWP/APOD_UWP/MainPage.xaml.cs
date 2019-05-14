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
using Windows.UI.Xaml.Media.Imaging;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace APOD_UWP
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        const string EndpointURL = "https://api.nasa.gov/planetary/apod";

        DateTime launchDate = new DateTime(1995, 6, 16);

        int imageCountToday;

        public MainPage()
        {
            this.InitializeComponent();

            MonthCalendar.MinDate = launchDate;
            MonthCalendar.MaxDate = DateTime.Today;
        }

        private void LaunchButton_Click(object sender, RoutedEventArgs e)
        {
            LimitRangeCheckBox.IsChecked = false;

            MonthCalendar.DataContext = launchDate;
        }

        private void LimitRangeCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            var firstDayOfThisYear = new DateTime(DateTime.Today.Year, 1, 1);
            MonthCalendar.MinDate = firstDayOfThisYear;
        }

        private void LimitRangeCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            MonthCalendar.MinDate = launchDate;
        }

        private async void MonthCalendar_DateChangedAsync(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args)
        {
            await RetrievePhoto();
        }

        private bool IsSupportedFormat(string photoURL)
        {
            var ext = Path.GetExtension(photoURL).ToLower();

            return (ext == ".jpg" || ext == ".jpeg" || ext == ".png" || ext == ".gif" ||
                    ext == ".tif" || ext == ".bmp" || ext == ".ico" || ext == ".svg");
        }

        private async Task RetrievePhoto()
        {
            var client = new HttpClient();
            JObject jResult = null;
            string responseContent = null;
            string description = null;
            string photoUrl = null;
            string copyright = null;

            // Set the UI elements to defaults
            ImageCopyrightTextBox.Text = "NASA";
            DescriptionTextBox.Text = "";

            // Build the date parameter string for the date selected, or the last date if a range is specified.
            DateTimeOffset dt = (DateTimeOffset)MonthCalendar.Date;

            string dateSelected = $"{dt.Year.ToString()}-{dt.Month.ToString("00")}-{dt.Day.ToString("00")}";
            string URLParams = $"?date={dateSelected}&api_key=DEMO_KEY";

            // Populate the HTTP client appropriately.
            client.BaseAddress = new Uri(EndpointURL);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // The critical call: send a GET request with the appropriate parameters.
            HttpResponseMessage response = client.GetAsync(URLParams).Result;

            if (response.IsSuccessStatusCode)
            {
                // Be ready to catch any data or server errors.
                try
                {
                    // Parse the response by using the Newtonsoft APIs.
                    responseContent = await response.Content.ReadAsStringAsync();

                    // Parse the response string for the details we need.
                    jResult = JObject.Parse(responseContent);

                    // Now get the image.
                    photoUrl = (string)jResult["url"];
                    var photoURI = new Uri(photoUrl);
                    var bmi = new BitmapImage(photoURI);

                    ImagePictureBox.Source = bmi;

                    if (IsSupportedFormat(photoUrl))
                    {
                        // Get the copyright message, but fill with "NASA" if no name is provided.
                        copyright = (string)jResult["copyright"];
                        if (copyright != null && copyright.Length > 0)
                        {
                            ImageCopyrightTextBox.Text = copyright;
                        }

                        // Populate the description text box.
                        description = (string)jResult["explanation"];
                        DescriptionTextBox.Text = description;
                    }
                    else
                    {
                        DescriptionTextBox.Text = $"Image type is not supported. URL is {photoUrl}";
                    }
                }
                catch (Exception ex)
                {
                    DescriptionTextBox.Text = $"Image data is not supported. {ex.Message}";
                }

                // Keep track of our downloads in case we reach the limit.
                ++imageCountToday;
                ImagesTodayTextBox.Text = imageCountToday.ToString();
            }
            else
            {
                DescriptionTextBox.Text = "We were unable to retrieve the NASA picture for that day: " +
                    $"{response.StatusCode.ToString()} {response.ReasonPhrase}";
            }
        }
    }
}
