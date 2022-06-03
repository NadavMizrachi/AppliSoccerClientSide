using AppliSoccerClientSide.Services;
using AppliSoccerClientSide.ViewModel;
using AppliSoccerObjects.Modeling;
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

    public partial class StaffDetails : ContentPage
    {
        public bool IsAdmin { get; set; }
        public bool IsCoach { get; set; }
        public TeamMember MyMember { get; set; }
        public TeamMember StaffToShow { get; set; }
        public ObservableCollection<ManagedRoleView> ManagedRoles { get; set; }

        public StaffDetails(TeamMember staffToShow)
        {
            InitializeComponent();
            StaffToShow = staffToShow;
            Title = CreateTitle(staffToShow);
            UpdateManagedRoles(StaffToShow);
            MyMember = ApplicationGlobalData.GetMyTeamMember();
            IsCoach = ((StaffAdditionalInfo)MyMember.AdditionalInfo).IsCoach;
            IsAdmin = (MyMember.MemberType == MemberType.Admin);
            if (IsAdmin)
            {
                AddAdminToolBarItems();
            }
            BindingContext = this;
        }

        private void UpdateManagedRoles(TeamMember staffToShow)
        {
            var staffAdditionalInfo = (StaffAdditionalInfo)staffToShow.AdditionalInfo;
            ManagedRoles = new ObservableCollection<ManagedRoleView>();
            bool isNoManagedRoles = (staffToShow.AdditionalInfo == null ||
                                    staffAdditionalInfo.ManagedRoles == null ||
                                    staffAdditionalInfo.ManagedRoles.Count == 0);
            if (isNoManagedRoles)
            {
                return;
            }
            List<ManagedRoleView> managedRolesViews =
               ManagedRoleView.CreateManagedRoleViews(staffAdditionalInfo.ManagedRoles);
            managedRolesViews.ForEach(roleStr => ManagedRoles.Add(roleStr));
        }

        private string CreateTitle(TeamMember staffToShow)
        {
            return staffToShow.FirstName + " " + staffToShow.LastName;
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



        private void EnableEditMode()
        {
            FirstNameEntry.IsReadOnly = false;
            LastNameEntry.IsReadOnly = false;
            PhoneNumberEntry.IsReadOnly = false;

            BirthdatePicker.IsVisible = true;
            BirthdateLabel.IsVisible = false;

            DescriptionEditor.IsReadOnly = false;
           
            EnableRoleEditting();

        }
        private async void RemoveButton_Clicked(object sender, EventArgs e)
        {
            bool yes = await DisplayAlert(
                "Edit confiramtion",
                $"Do you want to remove {StaffToShow.FirstName} {StaffToShow.LastName}?", "Yes", "No");
            Debug.WriteLine("Answer: " + yes);
            if (!yes)
            {
                return;
            }

            try
            {
                bool isRemoved = await AppliSoccerServerService.AppServer.RemoveMember(StaffToShow);
                if (isRemoved)
                {
                    await Navigation.PopAsync();
                }
                else
                {
                    await DisplayAlert("Error", "Cannot remove member", "Cancel");
                    return;
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Error has occurred during trying remove member", "Cancel");
            }
            return;
        }

        private void EditButton_Clicked(object sender, EventArgs e)
        {
            EnableEditMode();
            SaveButton.IsVisible = true;
        }

        private async void SaveButton_Clicked(object sender, EventArgs e)
        {
            bool yes = await DisplayAlert("Edit confiramtion", "Do you want to save the changes?", "Yes", "No");
            Debug.WriteLine("Answer: " + yes);
            if (!yes)
            {
                return;
            }

            // Send the new object to server
            try
            {
                TeamMember teamMemberNew = await AppliSoccerServerService.AppServer.UpdateTeamMember(StaffToShow);

                if (teamMemberNew != null)
                {
                    await Navigation.PopAsync();
                }
                else
                {
                    await DisplayAlert("Error", "Cannot save changes", "Cancel");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Cannot save changes", "Cancel");
            }
            return;

        }

        private void ChangeRoleButton_Clicked(object sender, EventArgs e)
        {
            EnableRoleEditting();
            SaveButton.IsVisible = true;

        }

        private void EnableRoleEditting()
        {
            ManagedRoleView.MarkAllAsVisibleForEditing(ManagedRoles);
        }
    }
}