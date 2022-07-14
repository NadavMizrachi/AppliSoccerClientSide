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
    public partial class NewEventPage : ContentPage
    {
        public TeamMember MyMember { get; set; }
        public EventDetailsViewModel EventDetails { get; set; }
        public NewEventPage()
        {
            InitializeComponent();
            InitMyMember();
            EventDetails = new EventDetailsViewModel();
            BindingContext = this;
        }

        public NewEventPage(DateTime date)
        {
            InitializeComponent();
            InitMyMember();
            EventDetails = new EventDetailsViewModel();
            EventDetails.Date = date;
            BindingContext = this;
        }
        private void InitMyMember()
        {
            MyMember = ApplicationGlobalData.GetMyTeamMember();
        }
        private async void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            // Convert view model to regular
            EventDetails eventDetails = 
                EventDetailsViewModel.ConvertToEventDetails(EventDetails, MyMember);
            // Validate Event details
            bool isValidEvent = await ValidateEvent(eventDetails);
            if (!isValidEvent)
            {
                return;
            }
            // Send to server
            CreateEventActionResult actionResult = 
                await AppliSoccerServerService.AppServer.CreateEvent(eventDetails);
            if(actionResult == null)
            {
                await DisplayAlert("Error!", "Unknown error!", "ok");
                return;
            }

            if(actionResult.Status == AppliSoccerObjects.ActionResults.Status.Success)
            {
                await DisplayAlert("Success!", "Event has been saved successfully!", "ok");
                await Shell.Current.GoToAsync($"//{nameof(SchedulePage)}");
            }
            else
            {
                ActionResultReporter actionResultReporter = new EventCreationResultReporter();
                await actionResultReporter.Report(actionResult, this);
                return;
            }
        }

        private Task<bool> ValidateEvent(EventDetails eventDetails)
        {
            return UIEventDetailsValidator.Validate(eventDetails, this);
        }
    }
}