using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using RestSharp;
using Newtonsoft.Json;

namespace Eventour
{
    public partial class MainPage : ContentPage
    {
        public class DataDisplay
        {
            public string Destination { get; set; }
            public string StartDate { get; set; }
            public string EndDate { get; set; }
            // public List<string> Attractions { get; set; }
            // public List<string> Ratings { get; set; }
            public string Attraction { get; set; }
            public string Rating { get; set; }
        }

        public MainPage()
        {
            InitializeComponent();

        }

        async void OnSuggestButtonClicked(object sender, EventArgs e)
        {
            var client = new RestClient();
            client.BaseUrl = new Uri("http://127.0.0.1:5000/test");

            var request = new RestRequest(Method.POST);
            request.AddParameter("dest", destination.Text);
            request.AddParameter("start_date", startDate.Text);
            request.AddParameter("end_date", endDate.Text);

            // request.AddParameter("dest", "Paris, France");
            // request.AddParameter("start_date", "01/02/2019");
            // request.AddParameter("end_date", "08/02/2019");

            // Send request to server and get response back
            IRestResponse response = client.Execute(request);
            // outputText.Text = response.Content;

            // // Parse JSON data from server
            // var results = JsonConvert.DeserializeObject<dynamic>(response.Content);
            // var dest = results.dest;
            // var start_date = results.start_date;
            // var end_date = results.end_date;
            // var attractions = results.attractions;
            // var ratings = results.ratings;

            // Different way to parse JSON data
            // List<DataDisplay> data = JsonConvert.DeserializeObject<List<DataDisplay>>(response.Content.ToString());
            // DataDisplay data = new DataDisplay();
            DataDisplay data = JsonConvert.DeserializeObject<DataDisplay>(response.Content);

            // // Ensure data from the server can be added to the new screen
            // App.Destination.Add(dest);
            // App.Destination.Add(start_date);
            // App.Destination.Add(end_date);
            // App.Destination.Add(attractions);
            // App.Destination.Add(ratings);

            // var searchResultsPage = new SearchResults(dest);
            // searchResultsPage.BindingContext = dest;

            // await Navigation.PushAsync(new SearchResults((string)data.Destination));
            await Navigation.PushAsync(new SearchResults("Paris, France", "01/02/2019", "08/02/2019", "Eiffel Tower", "4.6"));
            // await Navigation.PushAsync(new SearchResults(data.Destination, data.StartDate, data.EndDate, data.Attraction, data.Rating));
            // await Navigation.PushAsync(searchResultsPage);
        }
    }
}
