using AppliSoccerClientSide.Services;
using AppliSoccerClientSide.Views;
using AppliSoccerClientSide.Views.Schedule;
using AppliSoccerClientSide.Views.Tables;
using AppliSoccerObjects.ActionResults.LeagueActions;
using AppliSoccerObjects.Modeling;
using System;
using System.Collections.Generic;
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
            _permissionManager = ViewsPermissionManager.CreateManager();
        }

        private async void LoginButton_Clicked(object sender, System.EventArgs e)
        {
            if (await IsValidUser())
            {
                ApplicationGlobalData.InsertTeamMember(_loggedTeamMember);
                _permissionManager.UpdateUserPermissions(_loggedTeamMember, (Shell.Current as AppShell));
                await ApplicationGlobalData.Init();
                if (_loggedTeamMember.MemberType == MemberType.Admin)
                {
                    await Shell.Current.GoToAsync($"//{nameof(PlayersPage)}");
                }
                else
                {
                    await Shell.Current.GoToAsync($"//{nameof(SchedulePage)}");
                }
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