using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppliSoccerConnector.AppliSoccerServerConfig
{
    public class EventsConfig
    {
        private static readonly string _baseResource = "events";

        public static readonly string CreateEventPath = _baseResource + "/CreateEvent";
        public static readonly Method CreateEventMethod = Method.PUT;
        public static readonly string EventDetailsParamName = "eventDetails";

        public static readonly string GetEventsPath = _baseResource + "/GetEvents";
        public static readonly Method GetEventsMethod = Method.GET;
        public static readonly string GetEventsAskerIdParamName = "askerId";
        public static readonly string lowerBoundDateParamName = "lowerBoundDate";
        public static readonly string upBoundDateParamName = "upBoundDate";

        //[HttpGet]
        //public async Task<EditEventActionResult> EditEvent(EventDetails edittedEvent)
        public static readonly string EditEventPath = _baseResource + "/EditEvent";
        public static readonly Method EditEventMethod = Method.POST;


    }
}
