using System;
using System.Collections.Generic;

using Xamarin.Forms;
using RestSharp;

namespace Eventour
{
    public partial class SearchResults : ContentPage
    {
        // Object used to contain data for one trip
        public class DataDisplay
        {
            public string Dest { get; set; }
            public string StartDate { get; set; }
            public string EndDate { get; set; }
            public List<List<string>> Attractions { get; set; }
            public List<List<string>> Ratings { get; set; }
            public List<List<string>> ReviewCounts { get; set; }
            public List<List<string>> ImageURLs { get; set; }
            public List<List<string>> Durations { get; set; }
            public List<List<string>> Descriptions { get; set; }
            public List<List<string>> Addresses { get; set; }
        }

        // ===============================================
        // Global variables
        // ===============================================

        public static DataDisplay displayedData;
        // Create grid for every day in trip and every attraction on every day in trip
        List<List<Grid>> imgGrids = new List<List<Grid>>();
        List<List<Grid>> textGrids = new List<List<Grid>>();
        List<List<ImageButton>> attractionImgBtns = new List<List<ImageButton>>();
        List<List<ImageButton>> imgBtns = new List<List<ImageButton>>();
        // List<List<MinusImgBtnDecorator>> minusImgBtnDecorators = new List<List<MinusImgBtnDecorator>>();

        // List of attractions to remove
        List<string> attractionsToRemove = new List<string>();
        // List of days corresponding to attractions to be removed
        List<int> daysForAttractionsToRemove = new List<int>();

        public SearchResults(string dest, string startDate, string endDate, List<List<string>> attractions, List<List<string>> ratings, List<List<string>> reviewCounts, List<List<string>> imageURLs, List<List<string>> durations, List<List<string>> descriptions, List<List<string>> addresses)
        {
            InitializeComponent();

            // Store database query results in global variables
            displayedData = new DataDisplay
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

            DateTime startDateObj = DateTime.ParseExact(startDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            DateTime endDateObj = DateTime.ParseExact(endDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);

            destinationResult.Text = dest;
            startDateResult.Text = (startDateObj).ToString("MMM. dd, yyyy");
            endDateResult.Text = (endDateObj).ToString("MMM. dd, yyyy");

            // For each day in trip
            for (int i = 0; i < displayedData.Attractions.Count; i++) {
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
                List<ImageButton> currDayAttractionImgBtns = new List<ImageButton>();
                attractionImgBtns.Add(currDayAttractionImgBtns);
                List<ImageButton> currDayImgBtns = new List<ImageButton>();
                imgBtns.Add(currDayImgBtns);
                /* // Create new list of minus image button decorators for current day
                List<MinusImgBtnDecorator> currDayMinusImgBtnDecorators = new List<MinusImgBtnDecorator>();
                minusImgBtnDecorators.Add(currDayMinusImgBtnDecorators); */

                // For each attraction planned on current day in trip
                for (int j = 0; j < displayedData.Attractions[i].Count; j++)
                {
                    // Grid for text
                    var grid = new Grid { Padding = new Thickness(30, 0, 30, 30) };

                    // If no more attraction results to display (no more database results)
                    if (String.IsNullOrEmpty(displayedData.Attractions[i][j]))
                    {
                        // Create 1x1 grid for text
                        grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

                        // Heading
                        var noActivitiesLabel = new Label { Text = "No activities to display.", TextColor = Color.Black, HorizontalTextAlignment = TextAlignment.Start, VerticalTextAlignment = TextAlignment.Center};

                        // Add to grid
                        grid.Children.Add(noActivitiesLabel, 0, 0);
                        // Add to stack layout
                        SearchResultsStack.Children.Add(grid);

                        break;
                    }

                    // Create 4x5 grid for image
                    var imgGrid = new Grid { Padding = new Thickness(30, 20, 30, 20), HorizontalOptions = LayoutOptions.Fill, VerticalOptions = LayoutOptions.Fill };
                    imgGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(140) });
                    imgGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(60) });
                    imgGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    currDayImgGrid.Add(imgGrid);

                    // Boxviews
                    var currBoxview = new BoxView { CornerRadius = 10, HorizontalOptions = LayoutOptions.Fill, BackgroundColor = Color.FromHex("#72D5E6"), Opacity = 0.2 };
                    // Image
                    // string bindingContextAttractionImgBtn = String.Format("{0}~{1}", displayedData.Descriptions[i][j], displayedData.Addresses[i][j]);
                    string bindingContextAttractionImgBtn = String.Format("{0},{1}", i, j);
                    /* List<string> bindingContextAttractionImgBtn = new List<string>();
                    bindingContextAttractionImgBtn.Add(displayedData.Descriptions[i][j]);
                    bindingContextAttractionImgBtn.Add(displayedData.Addresses[i][j]); */

                    // Used to display images
                    var attractionImg = new ImageButton();
                    var noImageLabel = new Label();

                    // Check if image URL is a valid URL
                    Uri uriResult;
                    bool isImageURLValid = Uri.TryCreate(displayedData.ImageURLs[i][j], UriKind.Absolute, out uriResult)
                        && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

                    // If there is an image available
                    if (!String.IsNullOrEmpty(displayedData.ImageURLs[i][j]) && isImageURLValid == true)
                    {
                        attractionImg = new ImageButton
                        {
                            Source = displayedData.ImageURLs[i][j],
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

                    // Minus image button
                    string bindingContextMinusImgBtn = String.Format("{0},{1},"+ displayedData.Attractions[i][j], i, j);
                    // string bindingContextMinusImgBtn = String.Format("{0},{1}", i, displayedData.Attractions[i].FindIndex(displayedData.Attractions[i][j].Contains));
                    var minusImgBtn = new ImageButton
                    {
                        Source = "Minus.png", 
                        Aspect = Aspect.AspectFill, 
                        HorizontalOptions = LayoutOptions.End, 
                        BindingContext = bindingContextMinusImgBtn
                    };
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

                    // If there is an image available
                    if (!String.IsNullOrEmpty(displayedData.ImageURLs[i][j]) && isImageURLValid == true)
                    {
                        imgGrid.Children.Add(imgFrame, 0, 1, 0, 2);
                    }
                    else
                    {
                        imgGrid.Children.Add(noImageLabel, 0, 1, 0, 2);
                    }

                    imgGrid.Children.Add(minusImgBtn, 0, 1);

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
                    var currAttraction = new Label { Text = displayedData.Attractions[i][j].Trim(), TextColor = Color.Red };

                    // Heading
                    var ratingHeading = new Label { Text = "Rating: ", TextColor = Color.Black };
                    // Get rating for the current attraction
                    var currRating = new Label { Text = displayedData.Ratings[i][j].Trim(), TextColor = Color.Red };

                    // Heading
                    var reviewCountHeading = new Label { Text = "Number of Reviews: ", TextColor = Color.Black };
                    // Get number of reviews for the current attraction
                    var currReviewCount = new Label { Text = displayedData.ReviewCounts[i][j].Trim(), TextColor = Color.Red };

                    // Heading
                    var durationHeading = new Label { Text = "Estimated Duration: ", TextColor = Color.Black };

                    // Get estimated duration for the current attraction
                    string currDurationStr = "";

                    // If duration is not an empty string
                    if (!String.IsNullOrEmpty(displayedData.Durations[i][j].Trim()))
                    {
                        if (Int32.Parse(displayedData.Durations[i][j].Trim()) == 0)
                        {
                            currDurationStr = "< 1 hour";
                        }
                        else if (Int32.Parse(displayedData.Durations[i][j].Trim()) == 1)
                        {
                            currDurationStr = "1-2 hours";
                        }
                        else if (Int32.Parse(displayedData.Durations[i][j].Trim()) == 2)
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

        // Helper function - use to remove attractions the user wants to remove before navigating to a new page
        void RemoveAttractions()
        {
            for (int i = 0; i < attractionsToRemove.Count; i++)
            {
                int dayIdx = daysForAttractionsToRemove[i];
                int index = displayedData.Attractions[dayIdx].FindIndex(attractionsToRemove[i].Contains);

                if (index >= 0)
                {
                    // Remove attraction and associated information from global variable displayedData
                    displayedData.Attractions[dayIdx].RemoveAt(index);
                    displayedData.Ratings[dayIdx].RemoveAt(index);
                    displayedData.ReviewCounts[dayIdx].RemoveAt(index);
                    displayedData.ImageURLs[dayIdx].RemoveAt(index);
                    displayedData.Durations[dayIdx].RemoveAt(index);
                    displayedData.Descriptions[dayIdx].RemoveAt(index);
                    displayedData.Addresses[dayIdx].RemoveAt(index);
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

        // When user wants to save a trip
        async void OnSaveButtonClicked(object sender, EventArgs e)
        {
            // Remove any attractions user chose to remove before navigating to next page
            RemoveAttractions();

            DataDisplay data = displayedData;
            var tripsPage = new TripsPage(data.Dest, data.StartDate, data.EndDate, data.Attractions, data.Ratings, data.ReviewCounts, data.ImageURLs, data.Durations, data.Descriptions, data.Addresses);

            // Disable back button on next page
            NavigationPage.SetHasBackButton(tripsPage, false);
            await Navigation.PushAsync(tripsPage);
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

            var attractionDetailsPage = new AttractionDetails(displayedData.Descriptions[dayIdx][imgGridIdx], displayedData.Addresses[dayIdx][imgGridIdx], displayedData.Attractions[dayIdx][imgGridIdx]); 
            await Navigation.PushAsync(attractionDetailsPage);
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
            string attraction = indicesStrArr[2];

            /* var dayIdx = minusImgBtnDecorator.DayIndex;
            var imgGridIdx = minusImgBtnDecorator.ImgGridIndex;*/

            // Remove image and text grids from page
            SearchResultsStack.Children.Remove(imgGrids[dayIdx][imgGridIdx]);
            SearchResultsStack.Children.Remove(textGrids[dayIdx][imgGridIdx]);

            // Keep track of attractions to remove
            attractionsToRemove.Add(attraction);
            daysForAttractionsToRemove.Add(dayIdx);

            /* imgGrids[dayIdx].RemoveAt(imgGridIdx);
            textGrids[dayIdx].RemoveAt(imgGridIdx); */

            /* // Remove attraction and associated information from global variable
            displayedData.Attractions[dayIdx].RemoveAt(imgGridIdx);
            displayedData.Ratings[dayIdx].RemoveAt(imgGridIdx);
            displayedData.ReviewCounts[dayIdx].RemoveAt(imgGridIdx);
            displayedData.ImageURLs[dayIdx].RemoveAt(imgGridIdx);
            displayedData.Durations[dayIdx].RemoveAt(imgGridIdx);
            displayedData.Descriptions[dayIdx].RemoveAt(imgGridIdx);
            displayedData.Addresses[dayIdx].RemoveAt(imgGridIdx); */
        }

        // Go to new page when user wants to add attractions
        async void OnPlusImgBtnClicked(object sender, EventArgs e)
        {
            var client = new RestClient();
            // client.BaseUrl = new Uri("http://127.0.0.1:5000/");
            client.BaseUrl = new Uri("http://eventour.fun:5000/");

            int numDaysToQueryFor = 35;
            DateTime startDateObj = DateTime.ParseExact(startDateResult.Text, "MMM. dd, yyyy", System.Globalization.CultureInfo.InvariantCulture);
            DateTime endDateObj = DateTime.ParseExact(endDateResult.Text, "MMM. dd, yyyy", System.Globalization.CultureInfo.InvariantCulture);
            DateTime newEndDateObj = endDateObj.AddDays(numDaysToQueryFor);

            var request = new RestRequest("test", Method.POST);
            request.AddParameter("dest", destinationResult.Text);
            request.AddParameter("start_date", (startDateObj).ToString("dd/MM/yyyy"));
            request.AddParameter("end_date", (newEndDateObj).ToString("dd/MM/yyyy"));

            // Send request to server and get response back
            IRestResponse response = client.Execute(request);

            // Go to add attractions page if HTTP request was successful
            if (response.IsSuccessful == true)
            {
                // Deserialize JSON response from server
                DataDisplay data = Newtonsoft.Json.JsonConvert.DeserializeObject<DataDisplay>(response.Content);

                // Remove any attractions user chose to remove before navigating to next page
                RemoveAttractions();

                var addAttractionsPage = new AddAttractions(data.Dest, data.StartDate, (endDateObj).ToString("dd/MM/yyyy"), data.Attractions, data.Ratings, data.ReviewCounts, data.ImageURLs, data.Durations, data.Descriptions, data.Addresses);
                addAttractionsPage.BindingContext = displayedData;

                // Disable back button on page where user can add attractions
                NavigationPage.SetHasBackButton(addAttractionsPage, false);
                await Navigation.PushAsync(addAttractionsPage);
            }
            else
            {
                await DisplayAlert("Server Maintenance", "Please try again at a later time.", "OK");
            }
        }

        async void OnSuggestButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}
