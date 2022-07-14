using AppliSoccerObjects.Modeling;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppliSoccerClientSide.Services.Validators
{
    public class EventDetailsValidator
    {
        private EventDetails _eventDetails;
        public EventDetailsValidator(EventDetails eventDetails)
        {
            _eventDetails = eventDetails;
        }

        public bool IsValidTitle()
        {
            return _eventDetails.Title != null && _eventDetails.Title.Length > 0;
        }
        public bool IsValidStartTime()
        {
            return _eventDetails.StartTime != null;
        }

        public bool IsValidEndTime()
        {
            return _eventDetails.EndTime != null;
        }

        public bool IsValidCreatorId()
        {
            return _eventDetails.CreatorId != null && _eventDetails.CreatorId.Length > 0;
        }

        public bool IsValidTeamId()
        {
            return _eventDetails.TeamId != null && _eventDetails.TeamId.Length > 0;
        }

        public bool IsValidPlace()
        {
            return _eventDetails.Place != null && _eventDetails.Place.Name != null && _eventDetails.Place.Name.Length > 0;
        }

        public bool IsValidDescription()
        {
            return _eventDetails.Description != null && _eventDetails.Description.Length > 0;
        }

        public bool IsValidTimes()
        {
            if(_eventDetails.StartTime < DateTime.Now || _eventDetails.EndTime < DateTime.Now)
            {
                return false;
            }
            if(_eventDetails.StartTime > _eventDetails.EndTime)
            {
                return false;
            }

            if((_eventDetails.EndTime - _eventDetails.StartTime).TotalDays > 1)
            {
                return false;
            }

            if ((_eventDetails.EndTime - _eventDetails.StartTime).TotalDays < 0)
            {
                return false;
            }

            return true;
        }

    }
}
