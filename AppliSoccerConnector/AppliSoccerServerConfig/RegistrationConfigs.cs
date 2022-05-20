using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppliSoccerConnector.AppliSoccerServerConfig
{
    public class RegistrationConfigs
    {
        private static string _baseResource = "registration";
        
        public static string GetCountriesPath = _baseResource + "/GetCountries";
        public static Method GetCountriesMethod = Method.GET;

        public static string GetUnregisteredTeamsPath = _baseResource + "/GetUnregisteredTeams";
        public static Method GetUnregisteredTeamsMethod = Method.GET;
        public static string CountryParamName = "country";

        public static string RegisterTeamPath = _baseResource + "/RegisterTeam";
        public static Method RegisterTeamMethod = Method.POST;
        public static string TeamIdParamName = "teamId";
        public static string UsernameParamName = "username";
        public static string PasswordParamName = "password";


        /*
         request.AddParameter("teamId", teamId);
            request.AddParameter("username", adminUsername);
            request.AddParameter("password", adminPassword);*/

    }
}
