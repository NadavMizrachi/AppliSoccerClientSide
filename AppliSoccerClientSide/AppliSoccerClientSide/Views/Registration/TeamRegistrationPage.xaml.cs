using AppliSoccerClientSide.Services;
using AppliSoccerObjects.Modeling;
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
    public partial class TeamRegistrationPage : ContentPage
    {
        private string _country;
        public TeamRegistrationPage(string country)
        {
            _country = country;
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if(TeamPicker.Items != null && TeamPicker.Items.Count > 0)
            {
                return;
            }
            TeamPicker.IsEnabled = false;
            IEnumerable<TeamDetails> teamsDetails = await AppliSoccerServerService.AppServer.GetAvailableTeamNames(_country);
            TeamPicker.ItemsSource = await Task.Run( () => { return teamsDetails.ToList(); });
            TeamPicker.ItemDisplayBinding = new Binding("Name");
            TeamPicker.SelectedIndex = 0;
            TeamPicker.IsEnabled = true;
        }

        //private async Task<List<TeamDetails>>(IE)
        private async void ContinueButton_Clicked(object sender, EventArgs e)
        {
            var teamDetails = (TeamDetails)TeamPicker.SelectedItem;
            await Navigation.PushAsync(new TeamAdminRegistrationPage(teamDetails));
        }
    }
}