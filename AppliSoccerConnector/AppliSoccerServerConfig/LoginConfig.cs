using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppliSoccerConnector.AppliSoccerServerConfig
{
    public class LoginConfig
    {
        private static string _baseResource = "login";

        public static string LoginPath = _baseResource + "/Login";
        public static Method LoginMethod = Method.GET;
        public static string UsernameParamName = "username";
        public static string PasswordParamName = "password";
    }
}
