using AppliSoccerClientSide.Services;
using AppliSoccerObjects.Modeling;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppliSoccerClientSide.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PlayerDetails : ContentPage
    {
        public bool IsAdmin { get; set; }
        public bool IsCoach { get; set; }
        public TeamMember MyMember { get; set; }
        public TeamMember PlayerToShow { get; set; }
        public List<string> RoleNames
        {
            get
            {
                return Enum.GetNames(typeof(Role)).ToList();
            }
        }
        public PlayerDetails(TeamMember playerToShow)
        {
            InitializeComponent();
            PlayerToShow = playerToShow;// TeamMemberCreator.CopyTeamMember(playerToShow);
            Title = CreateTitle();
            MyMember = ApplicationGlobalData.GetMyTeamMember();
            IsAdmin = (MyMember.MemberType == MemberType.Admin);
            if (IsAdmin)
            {
                AddAdminToolBarItems();
            }
            BindingContext = this;
            
        }

        private void AddAdminToolBarItems()
        {
            var editToolBarItem = new ToolbarItem() { Text = "Edit" };
            editToolBarItem.Clicked += this.EditButton_Clicked;

            var removeToolBarItem = new ToolbarItem() { Text = "Remove" };
            removeToolBarItem.Clicked += this.RemoveButton_Clicked;

            ToolbarItems.Add(editToolBarItem);
            ToolbarItems.Add(removeToolBarItem);
        }

        private string CreateTitle()
        {
            if(PlayerToShow.AdditionalInfo != null)
            {
                int playerNumber = ((PlayerAdditionalInfo)PlayerToShow.AdditionalInfo).Number;
                return "#" + playerNumber + " " + PlayerToShow.LastName;
            }
            else
            {
                return PlayerToShow.FirstName + " " + PlayerToShow.LastName;
            }
            
        }

        private void EnableEditMode()
        {
            FirstNameEntry.IsReadOnly = false;
            LastNameEntry.IsReadOnly = false;
            PhoneNumberEntry.IsReadOnly = false;
            
            BirthdatePicker.IsVisible = true;
            BirthdateLabel.IsVisible = false;

            DescriptionEditor.IsReadOnly = false;

            EnableRoleEditting();

            NumberEntry.IsReadOnly = false;
        }

        private void RemoveButton_Clicked(object sender, EventArgs e)
        {

        }

        private void EditButton_Clicked(object sender, EventArgs e)
        {
            EnableEditMode();
            SaveButton.IsVisible = true;
        }

        private async void SaveButton_Clicked(object sender, EventArgs e)
        {
            string action = await DisplayActionSheet("Save changes",
                "Do you want to save the changes?, " +
                "Yes", "No");
            
            if (action.Equals("No"))
            {
                return;
            }

            // Send the new object to server
            try
            {
                bool isUpdateSuccess = await AppliSoccerServerService.AppServer.UpdateTeamMember(PlayerToShow);
                if (!isUpdateSuccess)
                {
                    await DisplayAlert("Error", "Cannot save changes", "Cancel");
                }
            }catch(Exception ex)
            {
                await DisplayAlert("Error", "Cannot save changes", "Cancel");
            }
            return;

        }

        private void ChangeRoleButton_Clicked(object sender, EventArgs e)
        {
            EnableRoleEditting();
            SaveButton.IsVisible = true;
            
            RolePicker.Focus();
        }

        private void EnableRoleEditting()
        {
            RolePicker.IsVisible = true;
            RoleLabel.IsVisible = false;
        }
    }
}