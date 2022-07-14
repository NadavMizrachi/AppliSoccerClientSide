using AppliSoccerClientSide.Services;
using AppliSoccerClientSide.ViewModel;
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
    public partial class DayEventsPage : ContentPage
    {
        public DateTime SelectedDay { get; set; }
        public List<EventDetailsViewModel> Events { get; set; }

        public DayEventsPage(DateTime selectedDay, DayEventCollection<EventDetailsViewModel> dayEvents)
        {
            InitializeComponent();
            InitPermissionedElements();
            SelectedDay = selectedDay;
            Events = dayEvents;
            BindingContext = this;
        }
        private void InitPermissionedElements()
        {
            ViewsPermissionManager viewsPermissionManager =
                ViewsPermissionManager.CreateManager();
            if (viewsPermissionManager.IsPermissionedForNewEventButton)
            {
                var newOrderButtonToolBar = new ToolbarItem() { IconImageSource = ImageSource.FromResource("AppliSoccerClientSide.Images.add-task.png") };
                newOrderButtonToolBar.Clicked += async (sender, e) =>
                {
                    await Navigation.PushAsync(new NewEventPage(SelectedDay)); ;
                };
                ToolbarItems.Add(newOrderButtonToolBar);
            }

        }
        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            EventDetailsViewModel eventModel = (sender as Element).BindingContext as EventDetailsViewModel;
            await Navigation.PushAsync(new ExistEventPage(eventModel));
        }
    }
}