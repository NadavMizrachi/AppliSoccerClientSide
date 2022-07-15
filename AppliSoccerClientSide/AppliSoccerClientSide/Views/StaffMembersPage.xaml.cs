using AppliSoccerClientSide.Services;
using AppliSoccerObjects.Modeling;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppliSoccerClientSide.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StaffMembersPage : ContentPage
    {
        private bool _wasAppeared = false;

        public TeamMember MyMember { get; }
        public ObservableCollection<TeamMember> StaffMembers { get; set; }
        public StaffMembersPage()
        {
            InitializeComponent();
            BindingContext = this;
            StaffMembers = new ObservableCollection<TeamMember>();
            MyMember = ApplicationGlobalData.GetMyTeamMember();
            if (MyMember.MemberType == MemberType.Admin)
            {
                AddAdminToolBarItems();
            }
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (!_wasAppeared)
            {
                await PullStaffFromServer();
                _wasAppeared = true;
            }
            
        }

        private void AddAdminToolBarItems()
        {
            // Add 'Add' button to toolbar
            var addToolBarItem = new ToolbarItem() { IconImageSource = ImageSource.FromResource("AppliSoccerClientSide.Images.icons8-add-user-male-90.png") };
            addToolBarItem.Clicked += this.AddItem_Clicked;

            ToolbarItems.Add(addToolBarItem);
        }

        private async void AddItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NewStaffPage());
        }

        private async Task PullStaffFromServer()
        {
            IsBusy = true;
            var teamMembers = await AppliSoccerServerService.AppServer.PullTeamMembers(MyMember.TeamId);
            var staffMembersFromServer = teamMembers.Where(teamMember => teamMember.MemberType == MemberType.Staff).ToList();
            staffMembersFromServer.ForEach(member => StaffMembers.Add(member));
            if(staffMembersFromServer != null || staffMembersFromServer.Count > 0)
            {
                StaffMembersListView.ItemsSource = staffMembersFromServer;
            }
            IsBusy = false;
        }

        private async void StaffMembersListView_Refreshing(object sender, EventArgs e)
        {
            await PullStaffFromServer();
        }

        private async void StaffMembersListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            ((ListView)sender).IsEnabled = false;

            // don't do anything if we just de-selected the row.
            if (e.Item == null) return;
            // Deselect the item.
            //if (sender is ListView lv) lv.SelectedItem = null;

            var chosenStaffMember = ((ListView)sender).SelectedItem as TeamMember;
            var copiedObject = TeamMemberCreator.CopyTeamMember(chosenStaffMember);
            await Navigation.PushAsync(new StaffDetails(copiedObject));

            ((ListView)sender).IsEnabled = true;
        }
    }
}