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
        public TeamMember MyMember { get; set; }
        public ObservableCollection<TeamMember> PlayerMembers { get; set; }

        private bool _wasAppeared = false;

        public PlayersPage()
        {
            InitializeComponent();
            BindingContext = this;
            PlayerMembers = new ObservableCollection<TeamMember>();
            MyMember = ApplicationGlobalData.GetMyTeamMember();
            if (MyMember.MemberType == MemberType.Admin)
            {
                AddAdminToolBarItems();
            }
            BindingContext = this;
            //PullPlayersFromServer().Wait();
        }


        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (!_wasAppeared)
            {
                await AddPlayersFromServerToUI();
                _wasAppeared = true;
            }
            
        }

        private void AddAdminToolBarItems()
        {
            // Add 'Add' button to toolbar
            var addToolBarItem = new ToolbarItem() { Text = "Add" };
            addToolBarItem.Clicked += this.AddItem_Clicked;

            ToolbarItems.Add(addToolBarItem);
        }
        private async void AddItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NewPlayerPage());
        }



        private async Task AddPlayersFromServerToUI()
        {
            IsBusy = true;
            //var teamMembers = await AppliSoccerServerService.AppServer.PullTeamMembers(MyMember.TeamId);
            //var playersFromServer = teamMembers.Where(teamMember => teamMember.MemberType == MemberType.Player).ToList();
            var playersFromServer = await PullPlayersFromServer();
            await Task.Run(() =>
            {
                Device.BeginInvokeOnMainThread( () => {
                    PlayerMembers.Clear();
                    playersFromServer.ForEach(member => PlayerMembers.Add(member));
                    PlayersListView.ItemsSource = null;
                    PlayersListView.ItemsSource = PlayerMembers;
                });
            });
            IsBusy = false;
        }


        private async Task<List<TeamMember>> PullPlayersFromServer()
        {
            var teamMembers = await AppliSoccerServerService.AppServer.PullTeamMembers(MyMember.TeamId);
            var playersFromServer = teamMembers.Where(teamMember => teamMember.MemberType == MemberType.Player).ToList();
            return playersFromServer;
        }

        // TODO: Prevent trigger openning player edit page when tapping fast on the same player
        private async void PlayersListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            ((ListView)sender).IsEnabled = false;
            
            // don't do anything if we just de-selected the row.
            if (e.Item == null) return;
            // Deselect the item.
            //if (sender is ListView lv) lv.SelectedItem = null;


            var chosenPlayer = ((ListView)sender).SelectedItem as TeamMember;
            var copiedObject = TeamMemberCreator.CopyTeamMember(chosenPlayer);
            await Navigation.PushAsync(new PlayerDetails(copiedObject));
            ((ListView)sender).IsEnabled = true;
        }

        private async void PlayersListView_Refreshing(object sender, EventArgs e)
        {
            await AddPlayersFromServerToUI();
        }
    }
}