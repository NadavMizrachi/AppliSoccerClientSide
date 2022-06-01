using AppliSoccerClientSide.Services;
using AppliSoccerObjects.Modeling;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppliSoccerClientSide.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PlayersPage : ContentPage
    {
        public TeamMember TeamMember { get; set; }
        public ObservableCollection<TeamMember> Players { get; set; }

        private bool _wasAppeared = false;

        public PlayersPage()
        {
            InitializeComponent();
            TeamMember = ApplicationGlobalData.GetMyTeamMember();
            BindingContext = this;
            if(TeamMember.MemberType == MemberType.Admin)
            {
                AddAdminToolBarItems();
            }
            Players = new ObservableCollection<TeamMember>();
        }



        private void AddAdminToolBarItems()
        {
            // Add 'Add' button to toolbar
            var addToolBarItem = new ToolbarItem() { Text = "Add" };
            addToolBarItem.Clicked += this.AddItem_Clicked;

            ToolbarItems.Add(addToolBarItem);
        }

        
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (!_wasAppeared)
            {
                await PullPlayersFromServer();
                _wasAppeared = true;
            }
        }

        private async Task PullPlayersFromServer()
        {
            IsBusy = true;
            var teamMembers = await AppliSoccerServerService.AppServer.PullTeamMembers(TeamMember.TeamId);
            var playersFromServer = teamMembers.Where(teamMember => teamMember.MemberType == MemberType.Player).ToList();
            playersFromServer.ForEach(member => Players.Add(member));
            PlayersListView.ItemsSource = Players;
            PlayersListView.ItemsSource = playersFromServer;
            IsBusy = false;
        }
   
        private async void PlayersListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            //((ListView)sender).SelectedItem = null;
        }

        

        // TODO: Prevent trigger openning player edit page when tapping fast on the same player
        private async void PlayersListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var chosenPlayer = ((ListView)sender).SelectedItem as TeamMember;
            var copiedObject = TeamMemberCreator.CopyTeamMember(chosenPlayer);
            await Navigation.PushAsync(new PlayerDetails(copiedObject));
        }


        private async void AddItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NewPlayerPage());
        }

        private async void PlayersListView_Refreshing(object sender, EventArgs e)
        {
            await PullPlayersFromServer();
        }
    }
}