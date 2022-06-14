using AppliSoccerClientSide.Exceptions;
using AppliSoccerClientSide.Services;
using AppliSoccerObjects.Modeling;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppliSoccerClientSide.Views.Registration
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TeamAdminRegistrationPage : ContentPage
    {
        private TeamDetails _teamDetails;
        private ViewsPermissionManager _permissionManager;

        public TeamAdminRegistrationPage(TeamDetails teamDetails)
        {
            InitializeComponent();
            _teamDetails = teamDetails;
            TeamNameLabel.Text = teamDetails.Name;
            _permissionManager = ViewsPermissionManager.CreateManager();
        }

        private async void RegisterButton_Clicked(object sender, EventArgs e)
        {
            if (!IsValidRegistrationDetails())
            {
                return;
            }
            try
            {
                TeamMember registeredUser = await Register();
                ApplicationGlobalData.Insert(registeredUser);
                _permissionManager.UpdateUserPermissions(registeredUser, (Shell.Current as AppShell));
                // Go to admin page
                await Shell.Current.GoToAsync($"//{nameof(PlayersPage)}");
            }
            catch (TeamRegistrationFailedException ex)
            {
                // Pop up message
                ShowErrorAlert("Team registration was failed!");
            }
            catch(Exception ex)
            {
                ShowErrorAlert("Exception has occurred!");
            }
        }

        private async void ShowErrorAlert(string msg)
        {
            await DisplayAlert("Error", msg, "OK");
        }

        private async Task<TeamMember> Register()
        {
            var teamId = _teamDetails.Id;
            var adminUsername = UsernameEntry.Text;
            var adminPassword = PasswordEntry.Text;
            var teamMember =  await AppliSoccerServerService.AppServer.RegisterTeam(teamId, adminUsername, adminPassword);
            if(teamMember == null)
            {
                throw new TeamRegistrationFailedException();
            }
            return teamMember;
        }

        private bool IsValidRegistrationDetails()
        {
            if (!AgreeCheckBox.IsChecked) { return false; }
            if(IsEmptyEntry(UsernameEntry)) { return false; }
            if(IsEmptyEntry(PasswordEntry)) { return false; }
            if (IsEmptyEntry(PasswordEntryValidation)) { return false; }
            
            var username = UsernameEntry.Text;
            var password = PasswordEntry.Text;
            var passwordValidation = PasswordEntryValidation.Text;
            
            if (!password.Equals(passwordValidation)) { return false; }

            RegistrationDetailsValidator registrationDetailsValidator =
                new RegistrationDetailsValidator(username, password, _teamDetails);
            return registrationDetailsValidator.IsValid();
        }

        private bool IsEmptyEntry(Entry entry)
        {
            return entry.Text == null || entry.Text.Length == 0;
        }


    }
}