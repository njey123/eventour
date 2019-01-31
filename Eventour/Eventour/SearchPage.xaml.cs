using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Eventour
{
    public partial class SearchPage : ContentPage
    {
        public SearchPage()
        {
            InitializeComponent();
        }

        async void OnSuggestButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SearchResults());
        }
    }
}
