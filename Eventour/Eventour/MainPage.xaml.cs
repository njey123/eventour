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

        public MainPage()
        {
            InitializeComponent();

        }

        async void OnSuggestButtonClicked(object sender, EventArgs e)
        {
            var client = new RestClient();
            // client.BaseUrl = new Uri("http://127.0.0.1:5000/");
            client.BaseUrl = new Uri("http://eventour.fun:5000/");

            var request = new RestRequest("test", Method.POST);
            request.AddParameter("dest", destination.Text);
            request.AddParameter("start_date", startDate.Text);
            request.AddParameter("end_date", endDate.Text);

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
                await DisplayAlert("Server Down for Maintenance", "Please try again at a later time.", "OK");
            }
        }

        async void OnAuthCompleted(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}
