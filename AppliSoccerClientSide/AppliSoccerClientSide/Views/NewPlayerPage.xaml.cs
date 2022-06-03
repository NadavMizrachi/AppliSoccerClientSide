using AppliSoccerClientSide.Services;
using AppliSoccerObjects.Modeling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppliSoccerClientSide.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewPlayerPage : ContentPage
    {
        public TeamMember MyTeamMember { get; set; }
        public User NewUser { get; set; }
        public NewPlayerPage()
        {
            InitializeComponent();
            MyTeamMember = ApplicationGlobalData.GetMyTeamMember();
            NewUser = UserCreator.CreateEmptyUserObject(MemberType.Player);
            BindingContext = this;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            RolePicker.SelectedIndex = (int)Role.GoalKeeper;
        }

        //private async Task<bool> ValidateNewPlayerAsync()
        //{
        //    RegistrationDetailsValidator validator =
        //        new RegistrationDetailsValidator(NewUser.Username, NewUser.Password, null);
        //    if (!validator.IsValidUsername())
        //    {
        //        await DisplayAlert("Details Error", "Username is not valid!", "cancel");
        //        return false;
        //    }
        //    if (!validator.IsValidPassword())
        //    {
        //        await DisplayAlert("Details Error", "Password is not valid!", "cancel");
        //        return false;
        //    }

        //    TeamMemberValidator teamMemberValidator =
        //        new TeamMemberValidator(NewUser.TeamMember);

        //    if (!teamMemberValidator.isValidFirstName())
        //    {
        //        await DisplayAlert("Details Error", "First Name is not valid!", "cancel");
        //        return false;
        //    }

        //    if (!teamMemberValidator.isValidLastName())
        //    {
        //        await DisplayAlert("Details Error", "Last Name is not valid!", "cancel");
        //        return false;
        //    }

        //    if (!teamMemberValidator.isValidPhoneNumber())
        //    {
        //        await DisplayAlert("Details Error", "Phone number is not valid!", "cancel");
        //        return false;
        //    }

        //    return true;
        //}

        private async void Button_Clicked(object sender, EventArgs e)
        {

            bool isValidPlayer =
                await UITeamMemberValidator.ValidateNewUser(NewUser, this);
            if (!isValidPlayer)
            {
                return;
            }

            try
            {
                UserCreator.PrepareUserForRegistration(MyTeamMember, NewUser);
                bool isCreationSucceed = await AppliSoccerServerService.AppServer.CreateUser(NewUser);
                if (isCreationSucceed)
                {
                    await Navigation.PopAsync();
                }
                else
                {
                    await DisplayAlert("Creation failed", "Tried to create the player, but something was wrong", "cancel");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error!", "Error was occured during trying new player", "cancel");
            }
            return;
        }
    }
}