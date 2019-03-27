using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Eventour
{
    public partial class AddAttractions : ContentPage
    {
        // ===============================================
        // Global variables
        // ===============================================

        // Create grid for every day in trip and every attraction on every day in trip
        List<List<Grid>> imgGrids = new List<List<Grid>>();
        List<List<Grid>> textGrids = new List<List<Grid>>();
        List<List<ImageButton>> attractionImgBtns = new List<List<ImageButton>>();
        List<List<ImageButton>> imgBtns = new List<List<ImageButton>>();

        // Data on page where user can add attractions
        SearchResults.DataDisplay addAttractionPageData;

        // Picker for date to add attractions to
        Picker datePicker = new Picker
        {
            Title = "Select a date to add activities for",
            VerticalOptions = LayoutOptions.CenterAndExpand
        };

        public AddAttractions(string dest, string startDate, string endDate, List<List<string>> attractions, List<List<string>> ratings, List<List<string>> reviewCounts, List<List<string>> imageURLs, List<List<string>> durations, List<List<string>> descriptions, List<List<string>> addresses)
        {
            InitializeComponent();

            // Store database query results in global variables
            addAttractionPageData = new SearchResults.DataDisplay
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

            // Grid for date
            var dateGrid = new Grid { Padding = new Thickness(30, 30, 30, 20) };
            dateGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            dateGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            DateTime startDateObj = DateTime.ParseExact(startDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            DateTime endDateObj = DateTime.ParseExact(endDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);

            // List of days in trip
            List<string> daysInTrip = new List<string>();
            for(int i = 0; i < (endDateObj - startDateObj).TotalDays + 1; i++)
            {
                DateTime currDateObj = startDateObj.AddDays(i);
                daysInTrip.Add((currDateObj).ToString("MMM. dd, yyyy"));
            }

            // Add items to picker
            foreach (string day in daysInTrip)
            {
                datePicker.Items.Add(day);
            }

            // Add to grid
            dateGrid.Children.Add(datePicker, 0, 0);
            // Add to stack layout
            AddAttractionsStack.Children.Add(dateGrid);

            bool areThereAttractionsLeft = true;
            // For each day in trip
            for (int i = 0; i < attractions.Count; i++)
            {
                // Create new list of grids for current day
                List<Grid> currDayImgGrid = new List<Grid>();
                imgGrids.Add(currDayImgGrid);
                List<Grid> currDayTextGrid = new List<Grid>();
                textGrids.Add(currDayTextGrid);
                List<ImageButton> currDayAttractionImgBtns = new List<ImageButton>();
                attractionImgBtns.Add(currDayAttractionImgBtns);
                List<ImageButton> currDayImgBtns = new List<ImageButton>();
                imgBtns.Add(currDayImgBtns);

                // For each attraction planned on current day in trip
                for (int j = 0; j < attractions[i].Count; j++)
                {
                    // Grid for text
                    var grid = new Grid { Padding = new Thickness(30, 0, 30, 30) };

                    // If no more attraction results to display (no more database results)
                    if (String.IsNullOrEmpty(attractions[i][j]))
                    {
                        // Create 1x1 grid for text
                        grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

                        // Heading
                        var noActivitiesLabel = new Label { Text = "No activities to display.", TextColor = Color.Black, HorizontalTextAlignment = TextAlignment.Start, VerticalTextAlignment = TextAlignment.Center };

                        // Add to grid
                        grid.Children.Add(noActivitiesLabel, 0, 0);
                        // Add to stack layout
                        AddAttractionsStack.Children.Add(grid);

                        areThereAttractionsLeft = false;
                        break;
                    }

                    // Create 4x5 grid for image
                    var imgGrid = new Grid { Padding = new Thickness(30, 20, 30, 20), HorizontalOptions = LayoutOptions.Fill, VerticalOptions = LayoutOptions.Fill };
                    imgGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(140) });
                    imgGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(60) });
                    imgGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    currDayImgGrid.Add(imgGrid);

                    // Boxviews
                    var currBoxview = new BoxView { CornerRadius = 10, HorizontalOptions = LayoutOptions.Fill, BackgroundColor = Color.FromHex("#72D5E6"), Opacity = 0.4 };

                    // Image
                    string bindingContextAttractionImgBtn = String.Format("{0},{1}", i, j);
                    var attractionImg = new ImageButton();
                    var noImageLabel = new Label();

                    // Check if image URL is a valid URL
                    Uri uriResult;
                    bool isImageURLValid = Uri.TryCreate(imageURLs[i][j], UriKind.Absolute, out uriResult)
                        && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

                    // If there is an image available
                    if (!String.IsNullOrEmpty(imageURLs[i][j]) && isImageURLValid == true)
                    {
                        attractionImg = new ImageButton
                        {
                            Source = imageURLs[i][j],
                            Aspect = Aspect.AspectFill,
                            HorizontalOptions = LayoutOptions.Fill,
                            VerticalOptions = LayoutOptions.Fill,
                            BindingContext = bindingContextAttractionImgBtn
                        };
                    }
                    // If no image available
                    else
                    {
                        attractionImg = new ImageButton
                        {
                            Aspect = Aspect.AspectFill,
                            HorizontalOptions = LayoutOptions.Fill,
                            VerticalOptions = LayoutOptions.Fill,
                            BindingContext = bindingContextAttractionImgBtn
                        };

                        // Heading
                        noImageLabel = new Label { Text = "No image found", TextColor = Color.Black, HorizontalTextAlignment = TextAlignment.Center, VerticalTextAlignment = TextAlignment.Center };
                    }
                    currDayAttractionImgBtns.Add(attractionImg);
                    attractionImgBtns[i][j].Clicked += OnAttractionImgBtnClicked;

                    /* var attractionImg = new ImageButton { Source = imageURLs[i][j], Aspect = Aspect.AspectFill, HorizontalOptions = LayoutOptions.Fill, VerticalOptions = LayoutOptions.Fill, BindingContext = bindingContextAttractionImgBtn };
                    currDayAttractionImgBtns.Add(attractionImg);
                    attractionImgBtns[i][j].Clicked += OnAttractionImgBtnClicked; */

                    // Plus image button
                    string bindingContextPlusImgBtn = String.Format("{0},{1}", i, j);
                    var plusImgBtn = new ImageButton { Source = "Plus.png", Aspect = Aspect.AspectFill, HorizontalOptions = LayoutOptions.End, BindingContext = bindingContextPlusImgBtn };
                    currDayImgBtns.Add(plusImgBtn);
                    // Event handler - when click minus button
                    imgBtns[i][j].Clicked += OnImgBtnClicked;

                    // Frame with rounded corners for image
                    var imgFrame = new Frame
                    {
                        Content = attractionImg,
                        CornerRadius = 10,
                        Margin = new Thickness(0),
                        Padding = new Thickness(0),
                        IsClippedToBounds = true
                    };

                    // Add to image grid
                    imgGrid.Children.Add(currBoxview, 0, 1, 0, 2);

                    // If there is an image available
                    if (!String.IsNullOrEmpty(imageURLs[i][j]) && isImageURLValid == true)
                    {
                        imgGrid.Children.Add(imgFrame, 0, 1, 0, 2);
                    }
                    else
                    {
                        imgGrid.Children.Add(noImageLabel, 0, 1, 0, 2);
                    }

                    imgGrid.Children.Add(plusImgBtn, 0, 1);

                    // Create 3x2 grid for text
                    grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                    grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                    grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                    grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(7, GridUnitType.Star) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(5, GridUnitType.Star) });
                    currDayTextGrid.Add(grid);

                    // Heading
                    var attractionHeading = new Label { Text = "Attraction: ", TextColor = Color.Black };
                    // Get current attraction for the current day in trip
                    var currAttraction = new Label { Text = attractions[i][j].Trim(), TextColor = Color.Red };

                    // Heading
                    var ratingHeading = new Label { Text = "Rating: ", TextColor = Color.Black };
                    // Get rating for the current attraction
                    var currRating = new Label { Text = ratings[i][j].Trim(), TextColor = Color.Red };

                    // Heading
                    var reviewCountHeading = new Label { Text = "Number of Reviews: ", TextColor = Color.Black };
                    // Get number of reviews for the current attraction
                    var currReviewCount = new Label { Text = reviewCounts[i][j].Trim(), TextColor = Color.Red };

                    // Heading
                    var durationHeading = new Label { Text = "Estimated Duration: ", TextColor = Color.Black };

                    // Get estimated duration for the current attraction
                    string currDurationStr = "";

                    // If duration is not an empty string
                    if (!String.IsNullOrEmpty(durations[i][j].Trim()))
                    {
                        if (Int32.Parse(durations[i][j].Trim()) == 0)
                        {
                            currDurationStr = "< 1 hour";
                        }
                        else if (Int32.Parse(durations[i][j].Trim()) == 1)
                        {
                            currDurationStr = "1-2 hours";
                        }
                        else if (Int32.Parse(durations[i][j].Trim()) == 2)
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
                    AddAttractionsStack.Children.Add(imgGrid);
                    AddAttractionsStack.Children.Add(grid);
                }

                if (areThereAttractionsLeft == false)
                {
                    break;
                }
            }
        }

        // Add attraction on certain day when plus button is clicked
        void OnImgBtnClicked(object sender, EventArgs e)
        {
            // If a date has not been chosen
            if (datePicker.SelectedItem == null)
            {
                DisplayAlert("Add Failed", "Please select a date.", "OK");
            }
            else
            {
                var imgBtn = sender as ImageButton;

                // Get date to add attractions to
                string chosenDate = datePicker.SelectedItem.ToString();
                // test.Text = chosenDate;

                string startDate = SearchResults.displayedData.StartDate;
                DateTime startDateObj = DateTime.ParseExact(startDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                DateTime chosenDateObj = DateTime.ParseExact(chosenDate, "MMM. dd, yyyy", System.Globalization.CultureInfo.InvariantCulture);

                int chosenDateIdx = (int)(chosenDateObj - startDateObj).TotalDays;

                // Get attraction to add to trip
                string indicesStr = imgBtn.BindingContext as string;
                string[] indicesStrArr = indicesStr.Split(',');
                int dayIdx = Int32.Parse(indicesStrArr[0]);
                int imgGridIdx = Int32.Parse(indicesStrArr[1]);

                // If user wants to add an attraction
                if (String.Equals(imgBtn.Source.ToString(), "File: Plus.png"))
                {
                    // Add attraction and its associated information to chosen day in trip
                    SearchResults.displayedData.Attractions[chosenDateIdx].Add(addAttractionPageData.Attractions[dayIdx][imgGridIdx]);
                    SearchResults.displayedData.Ratings[chosenDateIdx].Add(addAttractionPageData.Ratings[dayIdx][imgGridIdx]);
                    SearchResults.displayedData.ReviewCounts[chosenDateIdx].Add(addAttractionPageData.ReviewCounts[dayIdx][imgGridIdx]);
                    SearchResults.displayedData.ImageURLs[chosenDateIdx].Add(addAttractionPageData.ImageURLs[dayIdx][imgGridIdx]);
                    SearchResults.displayedData.Durations[chosenDateIdx].Add(addAttractionPageData.Durations[dayIdx][imgGridIdx]);
                    SearchResults.displayedData.Descriptions[chosenDateIdx].Add(addAttractionPageData.Descriptions[dayIdx][imgGridIdx]);
                    SearchResults.displayedData.Addresses[chosenDateIdx].Add(addAttractionPageData.Addresses[dayIdx][imgGridIdx]);

                    // Change icon
                    imgBtn.Source = "Checkmark.png";
                }
                // If user wants to deselect an attraction
                else
                {
                    string attractionName = addAttractionPageData.Attractions[dayIdx][imgGridIdx];
                    int index = SearchResults.displayedData.Attractions[chosenDateIdx].FindIndex(attractionName.Contains);
                    // test.Text = index.ToString();

                    if (index >= 0)
                    {
                        // Remove attraction and its associated information from chosen day in trip
                        SearchResults.displayedData.Attractions[chosenDateIdx].RemoveAt(index);
                        SearchResults.displayedData.Ratings[chosenDateIdx].RemoveAt(index);
                        SearchResults.displayedData.ReviewCounts[chosenDateIdx].RemoveAt(index);
                        SearchResults.displayedData.ImageURLs[chosenDateIdx].RemoveAt(index);
                        SearchResults.displayedData.Durations[chosenDateIdx].RemoveAt(index);
                        SearchResults.displayedData.Descriptions[chosenDateIdx].RemoveAt(index);
                        SearchResults.displayedData.Addresses[chosenDateIdx].RemoveAt(index);
                    }

                    // Change icon
                    imgBtn.Source = "Plus.png";
                }
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

        // When user is done adding attractions
        async void OnDoneButtonClicked(object sender, EventArgs e)
        {
            SearchResults.DataDisplay data = SearchResults.displayedData;
            var searchResultsPage = new SearchResults(data.Dest, data.StartDate, data.EndDate, data.Attractions, data.Ratings, data.ReviewCounts, data.ImageURLs, data.Durations, data.Descriptions, data.Addresses);

            // Disable back button on next page
            NavigationPage.SetHasBackButton(searchResultsPage, false);
            await Navigation.PushAsync(searchResultsPage);
        }

        // Show detailed information about attraction
        async void OnAttractionImgBtnClicked(object sender, EventArgs e)
        {
            var imgBtn = sender as ImageButton;

            // Get description and address for attraction
            string indicesStr = imgBtn.BindingContext as string;
            string[] indicesStrArr = indicesStr.Split(',');
            int dayIdx = Int32.Parse(indicesStrArr[0]);
            int imgGridIdx = Int32.Parse(indicesStrArr[1]);
            string attraction = indicesStr[2].ToString();

            var attractionDetailsPage = new AttractionDetails(addAttractionPageData.Descriptions[dayIdx][imgGridIdx], addAttractionPageData.Addresses[dayIdx][imgGridIdx], addAttractionPageData.Attractions[dayIdx][imgGridIdx]);
            await Navigation.PushAsync(attractionDetailsPage);
        }

        async void OnPlusImgBtnClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}
