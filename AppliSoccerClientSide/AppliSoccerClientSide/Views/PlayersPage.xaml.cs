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
            PullPlayersFromServer();
        }

        private void AddAdminToolBarItems()
        {
            // Add 'Add' button to toolbar
            var addToolBarItem = new ToolbarItem() { Text = "Add" };
            addToolBarItem.Clicked += this.AddItem_Clicked;

            ToolbarItems.Add(addToolBarItem);
        }

        //protected override async void OnAppearing()
        //{
        //    base.OnAppearing();
        //    try
        //    {
        //        await
        //    }catch(Exception ex)
        //    {
        //        Debug.WriteLine("There was exception during pulling players for server. Exception details: " + ex.Message);
        //    }
        //}

        private void PullPlayersFromServer()
        {
            //var teamMembers = await AppliSoccerServerService.AppServer.PullTeamMembers(TeamMember.TeamId);
            //var players = teamMembers.Select(teamMember => teamMember.MemberType == MemberType.Player);
            //// Set to UI list
            Task.Run(() =>
              {
                  List<TeamMember> members = new List<TeamMember>();
                  members.Add(new TeamMember() { 
                      FirstName = "Itay",
                      LastName = "Shechter",
                      BirthDate = DateTime.Now, 
                      Description = "Very good player",
                      ID = "ssd",
                      PhoneNumber = "0525958889",
                      AdditionalInfo = new PlayerAdditionalInfo()
                      {
                          Number = 9,
                          Role = Role.Attacker
                      }
                  });
                  members.Add(new TeamMember() { FirstName = "Dor", LastName = "Micha" });
                  members.Add(new TeamMember() { FirstName = "Miguel", LastName = "Vitor" });
                  members.Add(new TeamMember() { FirstName = "Miguel", LastName = "Vitor" });
                  members.Add(new TeamMember() { FirstName = "Miguel", LastName = "Vitor" });
                  members.Add(new TeamMember() { FirstName = "Miguel", LastName = "Vitor" });
                  members.Add(new TeamMember() { FirstName = "Miguel", LastName = "Vitor" });
                  members.Add(new TeamMember() { FirstName = "Miguel", LastName = "Vitor" });
                  members.Add(new TeamMember() { FirstName = "Miguel", LastName = "Vitor" });
                  members.Add(new TeamMember() { FirstName = "Miguel", LastName = "Vitor" });
                  members.Add(new TeamMember() { FirstName = "Miguel", LastName = "Vitor" });
                  members.Add(new TeamMember() { FirstName = "Miguel", LastName = "Vitor" });
                  members.ForEach(member => Players.Add(member));
                  PlayersListView.ItemsSource = Players;
              }).Wait();
            
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
    }
}