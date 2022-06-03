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
    public partial class NewStaffPage : ContentPage
    {

        private HashSet<Role> _roles;
        private bool _isCoach;
        private bool _showManagedRolePicker;
        public User NewUser { get; set; }
        private TeamMember MyTeamMember { get; set; }
        public bool IsCoach
        {
            get { return _isCoach; }
            set
            {
                if (_isCoach != value)
                {
                    _isCoach = value;

                    ShowManagedRolePicker = !value;
                    OnPropertyChanged("IsCoach");
                }
            }
        }
        public bool ShowManagedRolePicker
        {
            get { return _showManagedRolePicker; }
            set
            {
                if (_showManagedRolePicker != value)
                {
                    _showManagedRolePicker = value;
                    OnPropertyChanged("ShowManagedRolePicker");
                }
            }
        }
        
        public NewStaffPage()
        {
            InitializeComponent();
            ShowManagedRolePicker = true;
            _roles = new HashSet<Role>();
            MyTeamMember = ApplicationGlobalData.GetMyTeamMember();
            NewUser = UserCreator.CreateEmptyUserObject(MemberType.Staff);
            BindingContext = this;
        }


        private void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;
            Role roleOfCheckBox = GetRoleOfCheckBox(checkBox);
            if (checkBox.IsChecked)
            {
                _roles.Add(roleOfCheckBox);
            }
            else
            {
                _roles.Remove(roleOfCheckBox);
            }
        }

        private Role GetRoleOfCheckBox(CheckBox checkBox)
        {
            // Get "brother" label text, and by this string, infer the enum value
            Layout parent = checkBox.Parent as Layout;
            String enumStringValue = null;
            foreach (var child in parent.Children)
            {
                if (child.GetType() == typeof(Label))
                {
                    enumStringValue = (child as Label).Text;
                }
            }
            return (Role)Enum.Parse(typeof(Role), enumStringValue);
            
        }

        private async void MyListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            // don't do anything if we just de-selected the row.
            if (e.Item == null) return;

            // Optionally pause a bit to allow the preselect hint.
            await Task.Delay(500);

            // Deselect the item.
            if (sender is ListView lv) lv.SelectedItem = null;
        }

        private async void CreateButton_Clicked(object sender, EventArgs e)
        {
            AssignIsCoach();
            AssignManagedRolesToNewUser();
            bool isValidStaffMember =
                await UITeamMemberValidator.ValidateNewUser(NewUser, this);
            if (!isValidStaffMember)
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

        private void AssignIsCoach()
        {
            ((StaffAdditionalInfo)NewUser.TeamMember.AdditionalInfo).IsCoach = IsCoach;
        }

        private void AssignManagedRolesToNewUser()
        {
            var additionalIno = (StaffAdditionalInfo)NewUser.TeamMember.AdditionalInfo;
            if(additionalIno == null)
            {
                return;
            }
            additionalIno.ManagedRoles = GetManagedRolesTheUserChose();
        }

        private List<Role> GetManagedRolesTheUserChose()
        {
            return _roles.ToList();
        }
    }

}