using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppliSoccerConnector.AppliSoccerServerConfig
{
    public class RegistrationConfigs
    {
        private static string _baseResource = "registration";

        #region Get Countries
        public static string GetCountriesPath = _baseResource + "/GetCountries";
        public static Method GetCountriesMethod = Method.GET;
        #endregion

        #region Get Unregistered Teams
        public static string GetUnregisteredTeamsPath = _baseResource + "/GetUnregisteredTeams";
        public static Method GetUnregisteredTeamsMethod = Method.GET;
        public static string CountryParamName = "country";
        #endregion

        #region Register Team
        public static string RegisterTeamPath = _baseResource + "/RegisterTeam";
        public static Method RegisterTeamMethod = Method.POST;
        public static string TeamIdParamName = "teamId";
        public static string UsernameParamName = "username";
        public static string PasswordParamName = "password";
        #endregion

        #region Register User
        public static string RegisterUserPath = _baseResource + "/CreateUser";
        public static Method RegisterUserMethod = Method.PUT;
        // User is parameter for BODY ! 
        #endregion



    }
}
