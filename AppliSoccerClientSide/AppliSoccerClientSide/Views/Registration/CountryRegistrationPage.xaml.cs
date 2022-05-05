using AppliSoccerClientSide.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppliSoccerClientSide.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CountryRegistrationPage : ContentPage
    {

        
        public CountryRegistrationPage()
        {
            InitializeComponent();
            CountryPicker.IsEnabled = false;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            var countries = await AppliSoccerServerService.AppServer.GetAvailableCountriesAsync();
            foreach (var country in countries)
            {
                CountryPicker.Items.Add(country);
            }
            CountryPicker.SelectedIndex = 0;
            CountryPicker.IsEnabled = true;
        }
    }
}