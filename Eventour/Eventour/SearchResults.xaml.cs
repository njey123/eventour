using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Eventour
{
    public partial class SearchResults : ContentPage
    {
        // Create grid for every day in trip and every attraction on every day in trip
        List<List<Grid>> imgGrids = new List<List<Grid>>();
        List<List<Grid>> textGrids = new List<List<Grid>>();
        List<List<ImageButton>> imgBtns = new List<List<ImageButton>>();
        // List<List<MinusImgBtnDecorator>> minusImgBtnDecorators = new List<List<MinusImgBtnDecorator>>();


        public SearchResults(string dest, string startDate, string endDate, List<List<string>> attractions, List<List<string>> ratings, List<List<string>> reviewCounts, List<List<string>> imageURLs, List<List<string>> durations, List<List<string>> descriptions, List<List<string>> addresses)
        {
            InitializeComponent();

            DateTime startDateObj = DateTime.ParseExact(startDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            DateTime endDateObj = DateTime.ParseExact(endDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);

            destinationResult.Text = dest;
            startDateResult.Text = (startDateObj).ToString("MMM. dd, yyyy");
            endDateResult.Text = (endDateObj).ToString("MMM. dd, yyyy");

            // For each day in trip
            for (int i = 0; i < attractions.Count; i++) {
                var horizLine = new BoxView { HorizontalOptions = LayoutOptions.FillAndExpand, HeightRequest = 2, BackgroundColor = Color.FromHex("#F5F5F5") };
                SearchResultsStack.Children.Add(horizLine);

                // Create 1x2 grid
                var dayGrid = new Grid { Padding = new Thickness(30, 20, 30, 0) };
                dayGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                dayGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(7, GridUnitType.Star) });
                dayGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(5, GridUnitType.Star) });

                // Heading
                var dayHeading = new Label { Text = "Activities for: ", TextColor = Color.FromHex("#3ECCE5"), FontAttributes = FontAttributes.Bold };
                // Get current day
                DateTime currDateObj = startDateObj.AddDays(i);
                var currDay = new Label { Text = (currDateObj).ToString("MMM. dd, yyyy"), TextColor = Color.Red, FontAttributes = FontAttributes.Bold };

                // Add to grid
                dayGrid.Children.Add(dayHeading, 0, 0);
                dayGrid.Children.Add(currDay, 1, 0);

                // Add to stack layout
                SearchResultsStack.Children.Add(dayGrid);

                // Create new list of grids for current day
                List<Grid> currDayImgGrid = new List<Grid>();
                imgGrids.Add(currDayImgGrid);
                List<Grid> currDayTextGrid = new List<Grid>();
                textGrids.Add(currDayTextGrid);
                List<ImageButton> currDayImgBtns = new List<ImageButton>();
                imgBtns.Add(currDayImgBtns);
                /* // Create new list of minus image button decorators for current day
                List<MinusImgBtnDecorator> currDayMinusImgBtnDecorators = new List<MinusImgBtnDecorator>();
                minusImgBtnDecorators.Add(currDayMinusImgBtnDecorators); */

                // For each attraction planned on current day in trip
                for (int j = 0; j < attractions[i].Count; j++)
                {
                    // Create 4x5 grid for image
                    var imgGrid = new Grid { Padding = new Thickness(30, 20, 30, 20), HorizontalOptions = LayoutOptions.Fill, VerticalOptions = LayoutOptions.Fill };
                    imgGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(135) });
                    imgGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(65) });
                    imgGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    currDayImgGrid.Add(imgGrid);

                    // Boxviews
                    var currBoxview = new BoxView { CornerRadius = 10, HorizontalOptions = LayoutOptions.Fill, BackgroundColor = Color.FromHex("#72D5E6"), Opacity = 0.8 };
                    // Image
                    var attractionImg = new Image { Source = "https://media-cdn.tripadvisor.com/media/photo-w/0f/38/33/f6/beautiful-day-to-see.jpg", Aspect = Aspect.Fill, HorizontalOptions = LayoutOptions.Fill, VerticalOptions = LayoutOptions.Fill };

                    // Minus image button
                    string bindingContextMinusImgBtn = String.Format("{0},{1}", i, j);
                    var minusImgBtn = new ImageButton { Source = "Minus.png", Aspect = Aspect.Fill, HorizontalOptions = LayoutOptions.End, BindingContext = bindingContextMinusImgBtn};
                    currDayImgBtns.Add(minusImgBtn);
                    // Event handler - when click minus button
                    imgBtns[i][j].Clicked += OnMinusImgBtnClicked;

                    /* // Minus image button decorator
                    var minusImgBtnDecorator = new MinusImgBtnDecorator
                    {
                        DayIndex = i,
                        ImgGridIndex = j,
                        MinusImgBtn = minusImgBtn
                    };
                    // Add current minus image button decorator to list of minus image button decorators for the day
                    currDayMinusImgBtnDecorators.Add(minusImgBtnDecorator);
                    // minusImgBtnDecorators[i][j].MinusImgBtn.Clicked += OnMinusImgBtnClicked; */

                    // Frame with rounded corners for image
                    var imgFrame = new Frame { 
                        Content = attractionImg,
                        CornerRadius = 10, 
                        Margin = new Thickness(0), 
                        Padding = new Thickness(0), 
                        IsClippedToBounds = true 
                    };

                    // Add to image grid
                    imgGrid.Children.Add(currBoxview, 0, 1, 0, 2);
                    imgGrid.Children.Add(imgFrame, 0, 1, 0, 2);
                    imgGrid.Children.Add(minusImgBtn, 0, 1);

                    // Create 3x2 grid for text
                    var grid = new Grid { Padding = new Thickness(30, 0, 30, 30) };
                    grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star)});
                    grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                    grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                    grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(7, GridUnitType.Star) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(5, GridUnitType.Star) });
                    currDayTextGrid.Add(grid);

                    // Heading
                    var attractionHeading = new Label { Text = "Attraction: ", TextColor = Color.Black };
                    // Get current attraction for the current day in trip
                    var currAttraction = new Label { Text = attractions[i][j], TextColor = Color.Red };

                    // Heading
                    var ratingHeading = new Label { Text = "Rating: ", TextColor = Color.Black };
                    // Get rating for the current attraction
                    var currRating = new Label { Text = ratings[i][j], TextColor = Color.Red };

                    // Heading
                    var reviewCountHeading = new Label { Text = "Number of Reviews: ", TextColor = Color.Black };
                    // Get number of reviews for the current attraction
                    var currReviewCount = new Label { Text = reviewCounts[i][j], TextColor = Color.Red };

                    // Heading
                    var durationHeading = new Label { Text = "Estimated Duration: ", TextColor = Color.Black };

                    // Get estimated duration for the current attraction
                    string currDurationStr = "";

                    // If duration is not an empty string
                    if (!String.IsNullOrEmpty(durations[i][j]))
                    {
                        if (Int32.Parse(durations[i][j]) == 0)
                        {
                            currDurationStr = "< 1 hour";
                        }
                        else if (Int32.Parse(durations[i][j]) == 1)
                        {
                            currDurationStr = "1-2 hours";
                        }
                        else if (Int32.Parse(durations[i][j]) == 2)
                        {
                            currDurationStr = "2-3 hours";
                        }
                        else
                        {
                            currDurationStr = "> 3 hours";
                        }
                    }
                    var currDuration = new Label { Text = currDurationStr, TextColor = Color.Red };

                    // Add to grid
                    grid.Children.Add(attractionHeading, 0, 0);
                    grid.Children.Add(currAttraction, 1, 0);
                    grid.Children.Add(ratingHeading, 0, 1);
                    grid.Children.Add(currRating, 1, 1);
                    grid.Children.Add(reviewCountHeading, 0, 2);
                    grid.Children.Add(currReviewCount, 1, 2);
                    grid.Children.Add(durationHeading, 0, 3);
                    grid.Children.Add(currDuration, 1, 3);

                    // Add to stack layout
                    SearchResultsStack.Children.Add(imgGrid);
                    SearchResultsStack.Children.Add(grid);
                }
            }
        }

        // Wrapper (decorator) for minus image button
        public class MinusImgBtnDecorator
        {
            public int DayIndex { get; set; }
            public int ImgGridIndex { get; set; }
            public ImageButton MinusImgBtn { get; set; }
        }

        // Delete attraction on certain day when minus button is clicked
        void OnMinusImgBtnClicked(object sender, EventArgs e)
        {
            // var minusImgBtnDecorator = sender as MinusImgBtnDecorator;

            var imgBtn = sender as ImageButton;
            string indicesStr = imgBtn.BindingContext as string;
            string[] indicesStrArr = indicesStr.Split(',');
            int dayIdx = Int32.Parse(indicesStrArr[0]);
            int imgGridIdx = Int32.Parse(indicesStrArr[1]);

            /* var dayIdx = minusImgBtnDecorator.DayIndex;
            var imgGridIdx = minusImgBtnDecorator.ImgGridIndex;*/

            // Remove image and text grids from page
            SearchResultsStack.Children.Remove(imgGrids[dayIdx][imgGridIdx]);
            SearchResultsStack.Children.Remove(textGrids[dayIdx][imgGridIdx]);
        }

        async void OnSuggestButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}
