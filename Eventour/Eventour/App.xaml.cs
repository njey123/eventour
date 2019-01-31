using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Eventour
{
    public partial class App : Application
    {
        /* // Initialized properties below in App constructor
        public static IList<string> Destination { get; set; }
        public static IList<string> StartDate { get; set; }
        public static IList<string> EndDate { get; set; }
        public static IList<string> Attractions { get; set; }
        public static IList<string> Ratings { get; set; } */

        public App()
        {
            InitializeComponent();

            /* Destination = new List<string>();
            StartDate = new List<string>();
            EndDate = new List<string>();
            Attractions = new List<string>();
            Ratings = new List<string>(); */

            MainPage = new NavigationPage(new MainPage());
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
