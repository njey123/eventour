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

            // Deserialize JSON response from server
            DataDisplay data = Newtonsoft.Json.JsonConvert.DeserializeObject<DataDisplay>(response.Content);

            await Navigation.PushAsync(new SearchResults(data.Dest, data.StartDate, data.EndDate, data.Attractions[0][0], data.Ratings[0][0]));
        }
    }
}
