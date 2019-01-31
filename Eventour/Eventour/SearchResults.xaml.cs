using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Eventour
{
    public partial class SearchResults : ContentPage
    {
        public SearchResults(string dest, string start_date, string end_date, string attraction, string rating)
        {
            InitializeComponent();
            destinationResult.Text = dest;
            startDateResult.Text = start_date;
            endDateResult.Text = end_date;
            attractionResult.Text = attraction;
            ratingResult.Text = rating;
        }
        async void OnSuggestButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}
