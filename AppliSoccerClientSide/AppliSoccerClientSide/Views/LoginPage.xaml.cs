using AppliSoccerClientSide.Views;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppliSoccerClientSide.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private async void LoginButton_Clicked(object sender, System.EventArgs e)
        {
            if (IsValidUser())
            {
                await Shell.Current.GoToAsync($"//{nameof(SchedulePage)}");
            }
        }

        private bool IsValidUser()
        {
            if(UsernameEntry.Text == null || PasswordEntry.Text == null)
            {
                return false;
            }
            return UsernameEntry.Text.Equals("nadav") && PasswordEntry.Text.Equals("nadav");
        }

        private async void RegisterButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CountryRegistrationPage());
        }
    }
}