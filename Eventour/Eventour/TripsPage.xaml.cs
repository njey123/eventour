using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Eventour
{
    public partial class TripsPage : ContentPage
    {
        // ===============================================
        // Global variables
        // ===============================================

        // Data for all trips
        List<SearchResults.DataDisplay> AllTripsDataDisplayed = new List<SearchResults.DataDisplay>();
        // Data for one trip
        SearchResults.DataDisplay tripsPageData;

        public TripsPage(string dest, string startDate, string endDate, List<List<string>> attractions, List<List<string>> ratings, List<List<string>> reviewCounts, List<List<string>> imageURLs, List<List<string>> durations, List<List<string>> descriptions, List<List<string>> addresses)
        {
            InitializeComponent();

            // Store database query results in global variables
            tripsPageData = new SearchResults.DataDisplay
            {
                Dest = dest,
                StartDate = startDate,
                EndDate = endDate,
                Attractions = attractions,
                Ratings = ratings,
                ReviewCounts = reviewCounts,
                ImageURLs = imageURLs,
                Durations = durations,
                Descriptions = descriptions,
                Addresses = addresses
            };

            // Add data for trip to object that stores data for all trips
            AllTripsDataDisplayed.Add(tripsPageData);
        }

        // When logo button on top menu bar is clicked
        async void OnLogoBtnClicked(object sender, EventArgs e)
        {
            var mainPage = new MainPage();

            // Disable back button on next page
            NavigationPage.SetHasBackButton(mainPage, false);
            await Navigation.PushAsync(mainPage);
        }

        async void OnSaveButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}
