using AppliSoccerClientSide.Services;
using AppliSoccerClientSide.Views;
using AppliSoccerObjects.Modeling;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppliSoccerClientSide.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        private ViewsPermissionManager _permissionManager;
        private TeamMember _loggedTeamMember;
        public LoginPage()
        {
            InitializeComponent();
            _permissionManager = ViewsPermissionManager.CreateManager((Shell.Current as AppShell));
        }

        private async void LoginButton_Clicked(object sender, System.EventArgs e)
        {
            if (await IsValidUser())
            {
                ApplicationGlobalData.Insert(_loggedTeamMember);
                _permissionManager.UpdateUserPermissions(_loggedTeamMember);
                await Shell.Current.GoToAsync($"//{nameof(PlayersPage)}");
                //await Shell.Current.GoToAsync($"//{nameof(SchedulePage)}");
            }
            else
            {
                await DisplayAlert("Login error", "User is not valid!", "cancel");
            }
        }

        private async Task<bool> IsValidUser()
        {
            if (UsernameEntry.Text == null || PasswordEntry.Text == null)
            {
                return false;
            }
            try
            {
                IsBusy = true;
                _loggedTeamMember = await AppliSoccerServerService.AppServer.Login(UsernameEntry.Text, PasswordEntry.Text);
                IsBusy = false;
            }catch( Exception ex)
            {
                await DisplayAlert("Error", "Cannot login to server!", "cancel");
            }
            
            return _loggedTeamMember != null;
        }

        private async void RegisterButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CountryRegistrationPage());
        }
    }
}