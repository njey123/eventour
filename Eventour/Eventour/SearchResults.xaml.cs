using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Eventour
{
    public partial class SearchResults : ContentPage
    {
        public SearchResults(string dest, string startDate, string endDate, List<List<string>> attractions, List<List<string>> ratings, List<List<string>> reviewCounts, List<List<string>> imageURLs, List<List<string>> durations)
        {
            InitializeComponent();
            destinationResult.Text = dest;
            startDateResult.Text = startDate;
            endDateResult.Text = endDate;

            /* var grid = new Grid();
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            var image = new Image { Source = "LogoWords.png" }; */

            // For each day in trip
            for (var i = 0; i < attractions.Count; i++) {
                // For each attraction planned on current day in trip
                for (var j = 0; j < attractions[i].Count; j++)
                {
                    // Create 1x2 grid
                    var grid = new Grid { Padding = new Thickness(30, 20, 20, 0) };
                    grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star)});
                    grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(5, GridUnitType.Star) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(5, GridUnitType.Star) });

                    // Heading
                    var attractionHeading = new Label { Text = "Attraction: ", TextColor = Color.FromHex("#3ECCE5") };
                    // Get current attraction for the current day in trip
                    var currAttraction = new Label { Text = attractions[i][j], TextColor = Color.Red };

                    // Heading
                    var ratingHeading = new Label { Text = "Rating: ", TextColor = Color.FromHex("#3ECCE5") };
                    // Get rating for the current attraction
                    var currRating = new Label { Text = ratings[i][j], TextColor = Color.Red };

                    // Add to grid
                    grid.Children.Add(attractionHeading, 0, 0);
                    grid.Children.Add(currAttraction, 1, 0);
                    grid.Children.Add(ratingHeading, 0, 1);
                    grid.Children.Add(currRating, 1, 1);

                    // Add to stack layout
                    SearchResultsStack.Children.Add(grid);

                    /* // Create 1x2 grid
                    var grid2 = new Grid();
                    grid2.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(5, GridUnitType.Star) });
                    grid2.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(5, GridUnitType.Star) });

                    // Heading
                    var ratingHeading = new Label { Text = "Rating: ", TextColor = Color.FromHex("#3ECCE5") };

                    // Get rating for the current attraction
                    var currRating = new Label { Text = ratings[i][j], TextColor = Color.Red };

                    // Add to grid
                    grid2.Children.Add(ratingHeading, 0, 0);
                    grid2.Children.Add(currRating, 1, 0);

                    // Add to stack layout
                    SearchResultsStack.Children.Add(grid2); */
                }
            }
        }
        async void OnSuggestButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}
