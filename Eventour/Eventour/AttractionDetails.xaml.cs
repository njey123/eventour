using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Eventour
{
    public partial class AttractionDetails : ContentPage
    {
        public AttractionDetails(string description, string address, string attractionName)
        {
            InitializeComponent();

            // Create 1x1 grid
            var attractionGrid = new Grid { Padding = new Thickness(30, 30, 30, 0) };
            attractionGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            attractionGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

            // Create label
            var attractionLabel = new Label { Text = attractionName, TextColor = Color.Red, FontAttributes = FontAttributes.Bold, FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)) };
            // Add to grid
            attractionGrid.Children.Add(attractionLabel, 0, 0);
            // Add to stack layout
            AttractionDetailsStack.Children.Add(attractionGrid);

            // Create 1x1 grid
            var descriptionGrid = new Grid { Padding = new Thickness(30, 20, 30, 0) };
            descriptionGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            descriptionGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

            // If no description is availavble
            if (String.Equals(description.Trim(), ""))
            {
                description = "No description available.";
            }

            // Create label
            var descriptionLabel = new Label { Text = description, TextColor = Color.Black };
            // Add to grid
            descriptionGrid.Children.Add(descriptionLabel, 0, 0);
            // Add to stack layout
            AttractionDetailsStack.Children.Add(descriptionGrid);

            // Create 2x1 grid
            var addressGrid = new Grid { Padding = new Thickness(30, 20, 30, 0) };
            addressGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            addressGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            addressGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

            // Heading
            var addressHeading = new Label { Text = "Address: ", TextColor = Color.Black, FontAttributes = FontAttributes.Bold };

            // If no address is availavble
            if (String.Equals(address.Trim(), ""))
            {
                description = "No address available.";
            }

            // Create label
            var addressLabel = new Label { Text = address, TextColor = Color.Black };
            // Add to grid
            addressGrid.Children.Add(addressHeading, 0, 0);
            addressGrid.Children.Add(addressLabel, 0, 1);
            // Add to stack layout
            AttractionDetailsStack.Children.Add(addressGrid);
        }

        // When logo button on top menu bar is clicked
        async void OnLogoBtnClicked(object sender, EventArgs e)
        {
            var mainPage = new MainPage();

            // Disable back button on next page
            NavigationPage.SetHasBackButton(mainPage, false);
            await Navigation.PushAsync(mainPage);
        }
    }
}
