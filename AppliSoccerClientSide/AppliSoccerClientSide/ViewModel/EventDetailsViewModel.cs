using AppliSoccerClientSide.Helpers;
using AppliSoccerObjects.Modeling;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace AppliSoccerClientSide.ViewModel
{
    public class EventDetailsViewModel : INotifyPropertyChanged
    {
        private string _title;
        //private DateTime _startTime;
        //private DateTime _endTime;
        private DateTime _date;
        private TimeSpan _startTimeSpan;
        private TimeSpan _endTimeSpan;
        private string _description;
        private EventType _eventType;

        public string Title
        {
            get { return _title; }
            set { _title = value; OnPropertyChanged("Title"); }
        }
        //public DateTime StartTime
        //{
        //    get { return _startTime; }
        //    set { _startTime = value; OnPropertyChanged("StartTime"); }
        //}
        //public DateTime EndTime
        //{
        //    get { return _endTime; }
        //    set{ _endTime = value; OnPropertyChanged("EndTime"); }
        //}
        public DateTime Date
        {
            get { return _date; }
            set { _date= value; OnPropertyChanged("Date"); }
        }

        public TimeSpan StartTimeSpan
        {
            get { return _startTimeSpan; }
            set { _startTimeSpan = value; OnPropertyChanged("StartTimeSpan"); }
        }
        public TimeSpan EndTimeSpan
        {
            get { return _endTimeSpan; }
            set { _endTimeSpan = value; OnPropertyChanged("EndTimeSpan"); }
        }

        public string Description
        {
            get { return _description; } 
            set { _description = value; OnPropertyChanged("Description"); }
        }

        public EventType EventType
        {
            get { return _eventType; }
            set { _eventType = value; OnPropertyChanged("EventType"); }
        }

        public string Id { get; set; }

        public PlaceViewModel Place { get; set; }

        public EventDetailsViewModel()
        {
            //StartTime = DateTime.Now.Date;
            //EndTime = DateTime.Now.Date;
            Date = DateTime.Now.Date;
            StartTimeSpan = new TimeSpan(12, 0, 0);
            EndTimeSpan = new TimeSpan(13, 0, 0);
            Place = new PlaceViewModel();
        }


        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if(PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }


        public static EventDetails ConvertToEventDetails(EventDetailsViewModel eventViewModel, TeamMember creator)
        {
            if (eventViewModel == null) return null;
            if (creator == null) return null;
            if(eventViewModel.Place == null)
            {
                eventViewModel.Place = new PlaceViewModel();
            }
            return new EventDetails
            {
                CreatorId = creator.ID,
                Id = eventViewModel.Id,
                Title = eventViewModel.Title,
                Description = eventViewModel.Description,
                //StartTime = DateUtils.ToUtc(DateUtils.AddTimeSpanToDate(eventViewModel.StartTime, eventViewModel.StartTimeSpan)),
                //EndTime = DateUtils.ToUtc(DateUtils.AddTimeSpanToDate(eventViewModel.EndTime, eventViewModel.EndTimeSpan)),
                StartTime = DateUtils.ToUtc(DateUtils.AddTimeSpanToDate(eventViewModel.Date, eventViewModel.StartTimeSpan)),
                EndTime = DateUtils.ToUtc(DateUtils.AddTimeSpanToDate(eventViewModel.Date, eventViewModel.EndTimeSpan)),
                Place = new Place { Name = eventViewModel.Place.Name },
                TeamId = creator.TeamId,
                Type = eventViewModel.EventType
            };
        }

        public static List<EventDetailsViewModel> ConvertFromEventDetailsList(List<EventDetails> events)
        {
            return events.ConvertAll(e => ConvertFromEventDetails(e));
        }

        public static EventDetailsViewModel ConvertFromEventDetails(EventDetails eventDetails)
        {
            DateTime localStartTime = eventDetails.StartTime.ToLocalTime();
            DateTime localEndTime = eventDetails.EndTime.ToLocalTime();
            return new EventDetailsViewModel
            {
                Id = eventDetails.Id,
                Title = eventDetails.Title,
                Date = localStartTime.Date,
                StartTimeSpan = localStartTime.TimeOfDay,
                EndTimeSpan = localEndTime.TimeOfDay,
                Description = eventDetails.Description,
                EventType = eventDetails.Type,
                Place = new PlaceViewModel { Name = eventDetails.Place.Name }
            };
        }


    }
}
