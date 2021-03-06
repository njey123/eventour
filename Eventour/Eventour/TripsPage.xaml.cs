﻿using System;
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
        public static List<SearchResults.DataDisplay> AllTripsDataDisplayed = new List<SearchResults.DataDisplay>();

        // public static int tripIndex;

        // // Data for one trip
        // SearchResults.DataDisplay tripsPageData;

        // List of grids to display dates of trips
        List<Grid> dateGrids;
        // List of grids to display locations of trips
        List<Grid> destGrids;
        // List of destination labels
        public static List<Label> destLabels;
        // List of taps for destination labels
        public static List<TapGestureRecognizer> tapsList;

        public TripsPage()
        // public TripsPage(string dest, string startDate, string endDate, List<List<string>> attractions, List<List<string>> ratings, List<List<string>> reviewCounts, List<List<string>> imageURLs, List<List<string>> durations, List<List<string>> descriptions, List<List<string>> addresses)
        {
            InitializeComponent();

            /* // Store database query results in global variables
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
            AllTripsDataDisplayed.Add(tripsPageData); */

            // If no saved trips
            if (AllTripsDataDisplayed.Count == 0)
            {
                // Create 1x1 grid
                var textGrid = new Grid { Margin = new Thickness(30, 40, 30, 0) };
                textGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                textGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

                // Label
                var noTripsLabel = new Label { Text = "No trips to display.", TextColor = Color.Black };
                // Add to grid
                textGrid.Children.Add(noTripsLabel, 0, 0);
                // Add to stack layout
                TripsPageStack.Children.Add(textGrid);
            }

            // List of grids to display dates of trips
            dateGrids = new List<Grid>();
            // List of grids to display locations of trips
            destGrids = new List<Grid>();
            // List of destination labels
            destLabels = new List<Label>();
            // List of taps for destination labels
            tapsList = new List<TapGestureRecognizer>();

            /* // For every saved trip
            for (int k = 0; k < AllTripsDataDisplayed.Count; k++)
            { 
                tapsList.Add(new TapGestureRecognizer());
            } */

            // For every saved trip
            for (int k = 0; k < AllTripsDataDisplayed.Count; k++)
            {
                // Create 1x1 grid for dates
                var dateGrid = new Grid { Margin = new Thickness(30, 40, 30, 0)};
                dateGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                dateGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                dateGrids.Add(dateGrid);

                // For label
                string startDate = AllTripsDataDisplayed[k].StartDate;
                string endDate = AllTripsDataDisplayed[k].EndDate;

                /* DateTime startDateObj = DateTime.ParseExact(startDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                DateTime endDateObj = DateTime.ParseExact(endDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                var datesLabel = new Label { Text = (startDateObj).ToString("MM/dd/yyyy") + " - " + (endDateObj).ToString("MM/dd/yyyy"), TextColor = Color.FromHex("#3ECCE5"), FontAttributes = FontAttributes.Bold }; */

                var datesLabel = new Label { Text = startDate + " - " + endDate, TextColor = Color.FromHex("#3ECCE5"), FontAttributes = FontAttributes.Bold };
                // Add to grid
                dateGrid.Children.Add(datesLabel, 0, 0);

                // Create 1x1 grid for destination/ location
                var destGrid = new Grid { Margin = new Thickness(30, 10, 30, 0), Padding = new Thickness(1), HorizontalOptions = LayoutOptions.Fill, VerticalOptions = LayoutOptions.Fill };
                destGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(75) });
                destGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                destGrids.Add(destGrid);

                // Boxview
                var destBoxview = new BoxView { CornerRadius = 20, HorizontalOptions = LayoutOptions.Fill, BackgroundColor = Color.FromHex("#72D5E6"), Opacity = 0.2 };

                // Label - to be displayed on boxview
                string dest = AllTripsDataDisplayed[k].Dest;
                var destLabel = new Label
                { 
                    Text = dest, 
                    TextColor = Color.Red, 
                    FontAttributes = FontAttributes.Bold, 
                    FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)), 
                    HorizontalTextAlignment = TextAlignment.Center, 
                    VerticalTextAlignment = TextAlignment.Center,
                    BindingContext = k.ToString()
                };
                destLabels.Add(destLabel);

                TapGestureRecognizer tap = new TapGestureRecognizer();
                tap.Tapped += (sender, args) => {
                    // tripIndex = k;
                    OnSavedTripClicked(sender, args);
                };
                tapsList.Add(tap);
                destLabels[k].GestureRecognizers.Add(tapsList[k]);

                // Add to grid
                destGrid.Children.Add(destBoxview);
                destGrid.Children.Add(destLabel);

                // Add to stack layout
                TripsPageStack.Children.Add(dateGrid);
                TripsPageStack.Children.Add(destGrid);
            }
        }

        async void OnSavedTripClicked(object sender, EventArgs e)
        {
            var destLabel = sender as Label;
            string tripIndexStr = destLabel.BindingContext as string;
            int tripIndex = Int32.Parse(tripIndexStr);
            SearchResults.DataDisplay data = AllTripsDataDisplayed[tripIndex];
            if (SearchResults.displayedData != null)
            {
                var searchResultsPage = new SearchResults(data.Dest, data.StartDate, data.EndDate, data.Attractions, data.Ratings, data.ReviewCounts, data.ImageURLs, data.Durations, data.Descriptions, data.Addresses);

                // Disable back button on next page
                NavigationPage.SetHasBackButton(searchResultsPage, false);
                await Navigation.PushAsync(searchResultsPage);
            }

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

        async void OnPlanClicked(object sender, EventArgs e)
        {
            SearchResults.DataDisplay data = SearchResults.displayedData;
            if(SearchResults.displayedData != null)
            {
                var searchResultsPage = new SearchResults(data.Dest, data.StartDate, data.EndDate, data.Attractions, data.Ratings, data.ReviewCounts, data.ImageURLs, data.Durations, data.Descriptions, data.Addresses);

                // Disable back button on next page
                NavigationPage.SetHasBackButton(searchResultsPage, false);
                await Navigation.PushAsync(searchResultsPage);
            }

        }
        async void OnSearchClicked(object sender, EventArgs e)
        {
            var mainPage = new MainPage();

            // Disable back button on next page
            NavigationPage.SetHasBackButton(mainPage, false);
            await Navigation.PushAsync(mainPage);
        }

    }
}
