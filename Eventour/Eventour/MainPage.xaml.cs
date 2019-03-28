using System;
using System.Collections.Generic;
using RestSharp;
using Xamarin.Forms;
using Newtonsoft;


namespace Eventour
{
    public partial class MainPage : ContentPage
    {
        public class DataDisplay
        {
            public string Dest { get; set; }
            public string StartDate { get; set; }
            public string EndDate { get; set; }
            // public List<string> Attractions { get; set; }
            // public List<string> Ratings { get; set; }
            public List<List<string>> Attractions { get; set; }
            public List<List<string>> Ratings { get; set; }
            public List<List<string>> ReviewCounts { get; set; }
            public List<List<string>> ImageURLs { get; set; }
            public List<List<string>> Durations { get; set; }
            public List<List<string>> Descriptions { get; set; }
            public List<List<string>> Addresses { get; set; }
        }

        bool CalculateDateRange()
        {
            /* TimeSpan timeSpan = endDate.Date - startDate.Date;
            test.Text = timeSpan.Days.ToString();

            if (timeSpan.Days < 0 || timeSpan.Days > 7)
            {
                return false;
            }
            else
            {
                return true;
            } */

            // Format dates correctly
            string startDateStr = startDate.Date.ToString().Split(' ')[0];
            string endDateStr = endDate.Date.ToString().Split(' ')[0];

            // If month has only one digit
            if (Int32.Parse(startDateStr.Split('/')[0]) < 10)
            {
                startDateStr = "0" + startDateStr;
            }

            // If month has only one digit
            if (Int32.Parse(endDateStr.Split('/')[0]) < 10)
            {
                endDateStr = "0" + endDateStr;
            }

            // If day has only one digit
            if (Int32.Parse(startDateStr.Split('/')[1]) < 10)
            {
                startDateStr = startDateStr.Split('/')[0] + "/0" + startDateStr.Split('/')[1] + "/" + startDateStr.Split('/')[2];
            }

            // If day has only one digit
            if (Int32.Parse(endDateStr.Split('/')[1]) < 10)
            {
                endDateStr = endDateStr.Split('/')[0] + "/0" + endDateStr.Split('/')[1] + "/" + endDateStr.Split('/')[2];
            }

            DateTime startDateObj = DateTime.ParseExact(startDateStr, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            DateTime endDateObj = DateTime.ParseExact(endDateStr, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);

            // Calculate difference between selected dates
            int numDays = (endDateObj - startDateObj).Days;

            // Check if date range is valid
            if (numDays < 0 || numDays > 7)
            {
                return false;
            }
            return true;
        }

        public MainPage()
        {
            InitializeComponent();

        }

        async void OnSuggestButtonClicked(object sender, EventArgs e)
        {
            var client = new RestClient();
            // client.BaseUrl = new Uri("http://127.0.0.1:5000/");
            client.BaseUrl = new Uri("http://eventour.fun:5000/");

            // Format dates correctly
            string startDateStr = startDate.Date.ToString().Split(' ')[0];
            string endDateStr = endDate.Date.ToString().Split(' ')[0];

            // If month has only one digit
            if (Int32.Parse(startDateStr.Split('/')[0]) < 10)
            {
                startDateStr = "0" + startDateStr;
            }

            // If month has only one digit
            if (Int32.Parse(endDateStr.Split('/')[0]) < 10)
            {
                endDateStr = "0" + endDateStr;
            }

            // If day has only one digit
            if (Int32.Parse(startDateStr.Split('/')[1]) < 10)
            {
                startDateStr = startDateStr.Split('/')[0] + "/0" + startDateStr.Split('/')[1] + "/" + startDateStr.Split('/')[2];
            }

            // If day has only one digit
            if (Int32.Parse(endDateStr.Split('/')[1]) < 10)
            {
                endDateStr = endDateStr.Split('/')[0] + "/0" + endDateStr.Split('/')[1] + "/" + endDateStr.Split('/')[2];
            }

            DateTime startDateObj = DateTime.ParseExact(startDateStr, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            DateTime endDateObj = DateTime.ParseExact(endDateStr, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);

            var request = new RestRequest("test", Method.POST);
            request.AddParameter("dest", destination.Text);
            request.AddParameter("start_date", (startDateObj).ToString("dd/MM/yyyy"));
            request.AddParameter("end_date", (endDateObj).ToString("dd/MM/yyyy"));

            // Check if date range is valid
            bool isDateRangeValid = CalculateDateRange();
            if (isDateRangeValid)
            {
                // Send request to server and get response back
                IRestResponse response = client.Execute(request);

                // Go to search results page if HTTP request was successful
                if (response.IsSuccessful == true)
                {
                    // Deserialize JSON response from server
                    DataDisplay data = Newtonsoft.Json.JsonConvert.DeserializeObject<DataDisplay>(response.Content);

                    var searchResultsPage = new SearchResults(data.Dest, data.StartDate, data.EndDate, data.Attractions, data.Ratings, data.ReviewCounts, data.ImageURLs, data.Durations, data.Descriptions, data.Addresses);

                    // Disable back button on next page
                    NavigationPage.SetHasBackButton(searchResultsPage, false);
                    await Navigation.PushAsync(searchResultsPage);
                }
                else
                {
                    await DisplayAlert("Server Maintenance", "Please try again at a later time.", "OK");
                }
            }
            // If date range is not valid
            else
            {
                await DisplayAlert("Invalid Date Range", "Please pick a date range between 0 and 7 days.", "OK");
            }
        }

        void OnPlanClicked(object sender, EventArgs e)
        {

        }

        void OnTripClicked(object sender, EventArgs e)
        {

        }

        async void OnAuthCompleted(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}
