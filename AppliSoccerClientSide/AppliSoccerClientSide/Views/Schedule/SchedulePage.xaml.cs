using System;
using Xamarin.Plugin.Calendar.Models;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Collections.Generic;
using AppliSoccerObjects.Modeling;
using AppliSoccerClientSide.Services;
using System.Threading.Tasks;
using AppliSoccerClientSide.Views.ViewsUtil.ActionResultUIReporters;
using AppliSoccerClientSide.ViewModel;
using System.Linq;
using System.Windows.Input;
using System.ComponentModel;
using System.Threading;

namespace AppliSoccerClientSide.Views.Schedule
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SchedulePage : ContentPage
    {
        public TeamMember MyMember { get; set; }
        public DateTime Today { get; set; }
        public DateTime MaxDate { get; set; }
        public DateTime MinDate { get; set; }
        public EventCollection Events { get; set; }

        private bool _isRefreshing;
        public bool IsRefreshing
        {
            get { return _isRefreshing; }
            set { _isRefreshing = value; OnPropertyChanged(nameof(IsRefreshing)); }
        }

        public ICommand PullToRefreshCommand { get; set; }
        public ICommand ShowDayCommand { get; set; }
        public SchedulePage()
        {
            InitializeComponent();
            Events = new EventCollection();
            //InitPullToRefreshCommand();
            InitRefreshButton();
            InitShowDayCommand();
            InitMyMember();
            InitPermissionedElements();
            InitDatesOfUISchedule();
            BindingContext = this;

        }

        #region Inits
        //private void InitPullToRefreshCommand()
        //{
        //    PullToRefreshCommand = new Command(PullEventsFromServer);
        //}

        private void InitShowDayCommand()
        {
            ShowDayCommand = new Command(ShowDay);
        }

        private void InitMyMember()
        {
            MyMember = ApplicationGlobalData.GetMyTeamMember();
        }

        private void InitPermissionedElements()
        {
            ViewsPermissionManager viewsPermissionManager =
                ViewsPermissionManager.CreateManager();
            if (viewsPermissionManager.IsPermissionedForNewEventButton)
            {
                var newEventButtonToolBar = new ToolbarItem() { IconImageSource = ImageSource.FromResource("AppliSoccerClientSide.Images.add-task.png") };
                newEventButtonToolBar.Clicked += async (sender, e) =>
                {
                    await Navigation.PushAsync(new NewEventPage()); ;
                };
                ToolbarItems.Add(newEventButtonToolBar);
            }
            

        }

        private void InitRefreshButton()
        {
            var refreshButtonToolBar = new ToolbarItem() { IconImageSource = ImageSource.FromResource("AppliSoccerClientSide.Images.refresh.png") };
            refreshButtonToolBar.Clicked += async (sender, e) =>
            {
                await PullEventsFromServer();
            };
            ToolbarItems.Add(refreshButtonToolBar);
        }

        private void InitDatesOfUISchedule()
        {
            Today = DateTime.Now.ToLocalTime();
            MaxDate = Today.AddYears(1);
            MinDate = Today.AddYears(-1);
        } 
        #endregion

        private bool _wasAppeared = false;
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            if (_wasAppeared)
                return;
            _wasAppeared = true;
            await PullEventsFromServer();

        }

        private async Task PullEventsFromServer()
        {
            if (IsRefreshing) return;

            IsRefreshing = true;
            calender.SelectedDate = null;
            var actionRes = await AppliSoccerServerService.AppServer.GetAllEvents(MyMember.ID);
            if(actionRes == null)
            {
                await DisplayAlert("Error!", "Unknown error has occurred!", "ok");
                return;
            }
            if(actionRes.Status == AppliSoccerObjects.ActionResults.Status.Fail)
            {
                ActionResultReporter resultReporter = new GetEventsResultReporter();
                await resultReporter.Report(actionRes, this);
                return;
            }
            List<EventDetails> eventsFromServer = actionRes.Events;
            List<EventDetailsViewModel> eventsViewModels = EventDetailsViewModel.ConvertFromEventDetailsList(eventsFromServer);
            
            // Clean events
            foreach (var key in Events.Keys.ToList())
            {
                Events.Remove(key);
            }
            // Add the new events
            foreach (var eventViewModel in eventsViewModels)
            {
                DateTime dateKey = eventViewModel.Date;
                if (!Events.ContainsKey(dateKey))
                {
                    
                    Events.Add(dateKey, new DayEventCollection<EventDetailsViewModel>());
                }
                //EventModel eventModel = new EventModel { Name = "Name", Description = eventViewModel.Description };
                DayEventCollection<EventDetailsViewModel> dayEventsCollection = Events[dateKey] as DayEventCollection<EventDetailsViewModel>;
                dayEventsCollection.Add(eventViewModel);
            }

            IsRefreshing = false;
            //
        }

        private async void ShowDay()
        {
            if (!calender.SelectedDate.HasValue)
                return;
            DateTime selectedDay = calender.SelectedDate.Value;
            if (!Events.ContainsKey(selectedDay))
                return;
            DayEventCollection<EventDetailsViewModel> dayEvents = Events[selectedDay] as DayEventCollection<EventDetailsViewModel>;
            await Navigation.PushAsync(new DayEventsPage(selectedDay, dayEvents));
            calender.SelectedDate = null;
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            EventDetailsViewModel eventModel = (sender as Element).BindingContext as EventDetailsViewModel;
            await Navigation.PushAsync(new ExistEventPage(eventModel)); ;
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            EventDetailsViewModel eventModel = (sender as Element).BindingContext as EventDetailsViewModel;
            await Navigation.PushAsync(new ExistEventPage(eventModel)); ;
        }
    }

    public interface IPersonalizableDayEvent
    {
        #region PersonalizableProperties
        /// <summary>
        /// Color to use as indicator when there are events on the day
        /// if the EventIndicatorColor is null then the general EventIndicatorColor of the Calendar will be used
        /// </summary>
        Color? EventIndicatorColor { get; set; }

        /// <summary>
        /// Color to use as indicator when the day is selected and there are events on the day, if this is null then the EventIndicatorColor is used,
        /// if the EventIndicatorColor is null then the general EventIndicatorSelectedColor of the Calendar will be used
        /// </summary>
        Color? EventIndicatorSelectedColor { get; set; }

        /// <summary>
        /// Color for text to use as indicator when there are events on the day
        /// if the EventIndicatorTextColor is null then the general EventIndicatorTextColor of the Calendar will be used
        /// </summary>
        Color? EventIndicatorTextColor { get; set; }

        /// <summary>
        /// Color for text to use as indicator when the day is selected and there are events on the day, if this is null then the EventIndicatorTextColor is used,
        /// if the EventIndicatorSelectedTextColor is null then the general EventIndicatorSelectedTextColor of the Calendar will be used
        /// </summary>
        Color? EventIndicatorSelectedTextColor { get; set; }
        #endregion
    }

    public class DayEventCollection<T> : List<T>, IPersonalizableDayEvent
    {
        /// <summary>
        /// Empty contructor extends from base()
        /// </summary>
        public DayEventCollection() : base()
        { }

        /// <summary>
        /// Color contructor extends from base()
        /// </summary>
        /// <param name="eventIndicatorColor"></param>
        /// <param name="eventIndicatorSelectedColor"></param>
        public DayEventCollection(Color? eventIndicatorColor, Color? eventIndicatorSelectedColor) : base()
        {
            EventIndicatorColor = eventIndicatorColor;
            EventIndicatorSelectedColor = eventIndicatorSelectedColor;
        }

        public DayEventCollection(Color? eventIndicatorColor, Color? eventIndicatorSelectedColor, Color? eventText) : base()
        {
            EventIndicatorColor = eventIndicatorColor;
            EventIndicatorSelectedColor = eventIndicatorSelectedColor;
            EventIndicatorTextColor = eventText;
        }

        /// <summary>
        /// IEnumerable contructor extends from base(IEnumerable collection)
        /// </summary>
        /// <param name="collection"></param>
        public DayEventCollection(IEnumerable<T> collection) : base(collection)
        { }

        /// <summary>
        /// Capacity contructor extends from base(int capacity)
        /// </summary>
        /// <param name="capacity"></param>
        public DayEventCollection(int capacity) : base(capacity)
        { }

        #region PersonalizableProperties
        public Color? EventIndicatorColor { get; set; }
        public Color? EventIndicatorSelectedColor { get; set; }
        public Color? EventIndicatorTextColor { get; set; }
        public Color? EventIndicatorSelectedTextColor { get; set; }

        #endregion
    }
}