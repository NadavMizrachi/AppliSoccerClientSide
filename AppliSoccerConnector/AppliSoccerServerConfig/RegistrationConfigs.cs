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
    }
}
