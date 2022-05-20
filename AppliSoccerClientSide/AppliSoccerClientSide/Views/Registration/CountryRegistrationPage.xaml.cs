using AppliSoccerClientSide.Services;
using AppliSoccerClientSide.Views.Registration;
using System;


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
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (CountryPicker.Items != null && CountryPicker.Items.Count > 0)
            {
                return;
            }
            CountryPicker.IsEnabled = false;
            var countries = await AppliSoccerServerService.AppServer.GetAvailableCountriesAsync();
            foreach (var country in countries)
            {
                CountryPicker.Items.Add(country);
            }
            CountryPicker.SelectedIndex = 0;
            CountryPicker.IsEnabled = true;
        }

        private async void ContinueButton_Clicked(object sender, EventArgs e)
        {
            //await Navigation.PushAsync(new CountryRegistrationPage());
            await Navigation.PushAsync(new TeamRegistrationPage(CountryPicker.Items[CountryPicker.SelectedIndex]));
        }

    }
}