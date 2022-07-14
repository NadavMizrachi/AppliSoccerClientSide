using AppliSoccerClientSide.Services;
using AppliSoccerClientSide.ViewModel;
using AppliSoccerClientSide.Views.ViewsUtil;
using AppliSoccerClientSide.Views.ViewsUtil.ActionResultUIReporters;
using AppliSoccerObjects.ActionResults.EventsActions;
using AppliSoccerObjects.Modeling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppliSoccerClientSide.Views.Schedule
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ExistEventPage : ContentPage
    {
        public TeamMember MyMember { get; set; }
        public EventDetailsViewModel EventDetails { get; set; }
        public ExistEventPage(EventDetailsViewModel eventDetials)
        {
            InitializeComponent();
            EventDetails = eventDetials;
            InitMyMember();
            InitPermissionedElements();
            BindingContext = this;
        }

        private void InitMyMember()
        {
            MyMember = ApplicationGlobalData.GetMyTeamMember();
        }

        private void InitPermissionedElements()
        {
            ViewsPermissionManager viewsPermissionManager =
                ViewsPermissionManager.CreateManager();
            if (viewsPermissionManager.IsPermissionedForEditExistEvent)
            {
                var editButtonToolBar = new ToolbarItem() { IconImageSource = ImageSource.FromResource("AppliSoccerClientSide.Images.icons8-edit-calendar-100.png") };
                editButtonToolBar.Clicked +=  (sender, e) =>
                {
                    MakeElementsEditable();
                    ShowSaveBarButton();
                    this.titleEntry.Focus();
                };
                ToolbarItems.Add(editButtonToolBar);
            }

        }

        async void OnPreviousPageButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
        private void MakeElementsEditable()
        {
            titleEntry.IsReadOnly = false;
            datePicker.IsEnabled = true;
            startTimePicker.IsEnabled = true;
            endTimePicker.IsEnabled = true;
            placeNameEntry.IsReadOnly = false;
            eventTypePicker.IsEnabled = true;
            descriptionEditor.IsReadOnly = false;
        }

        private void ShowSaveBarButton()
        {
            if (ToolbarItems.Count == 2)
            {
                // Save button already was added
                return;
            }
            var saveButtonToolBar = new ToolbarItem() { IconImageSource = ImageSource.FromResource("AppliSoccerClientSide.Images.icons8-save-100.png") };
            saveButtonToolBar.Clicked += async (sender, e) =>
            {
                await SendEdittedDataToServer();
            };
            ToolbarItems.Insert(0, saveButtonToolBar);
        }

        private async Task SendEdittedDataToServer()
        {
            // Validate
            // Convert view model to regular
            EventDetails edittedEvent =
                EventDetailsViewModel.ConvertToEventDetails(EventDetails, MyMember);
            // Validate Event details
            bool isValidEvent = await ValidateEvent(edittedEvent);
            if (!isValidEvent)
            {
                return;
            }
            // Send to server
            EditEventActionResult actionResult =
                await AppliSoccerServerService.AppServer.EditEvent(edittedEvent);
            if(actionResult == null)
            {
                await DisplayAlert("Error!", "Unknown error has occurred!", "ok");
                return;
            }
            if (actionResult.Status == AppliSoccerObjects.ActionResults.Status.Success)
            {
                await DisplayAlert("Success!", "Event has been saved successfully!", "ok");
                await Shell.Current.GoToAsync($"//{nameof(SchedulePage)}");
            }
            else
            {
                ActionResultReporter actionResultReporter = new EventEdittingResultReporter();
                await actionResultReporter.Report(actionResult, this);
                return;
            }
            // Save changes in server
            //pop back
        }

        private Task<bool> ValidateEvent(EventDetails eventDetails)
        {
            return UIEventDetailsValidator.Validate(eventDetails, this);
        }
    }
}