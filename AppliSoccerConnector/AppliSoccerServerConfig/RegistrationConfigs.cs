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
        public static readonly string GetCountriesPath = _baseResource + "/GetCountries";
        public static readonly Method GetCountriesMethod = Method.GET;
        #endregion

        #region Get Unregistered Teams
        public static readonly string GetUnregisteredTeamsPath = _baseResource + "/GetUnregisteredTeams";
        public static readonly Method GetUnregisteredTeamsMethod = Method.GET;
        public static readonly string CountryParamName = "country";
        #endregion

        #region Register Team
        public static readonly string RegisterTeamPath = _baseResource + "/RegisterTeam";
        public static readonly Method RegisterTeamMethod = Method.POST;
        public static readonly string TeamIdParamName = "teamId";
        public static readonly string UsernameParamName = "username";
        public static readonly string PasswordParamName = "password";
        #endregion

        #region Register User
        public static readonly string RegisterUserPath = _baseResource + "/CreateUser";
        public static readonly Method RegisterUserMethod = Method.PUT;
        // User is parameter for BODY ! 
        #endregion

    }
}
