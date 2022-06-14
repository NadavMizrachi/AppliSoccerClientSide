using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppliSoccerConnector.AppliSoccerServerConfig
{
    public class LoginConfig
    {
        private static string _baseResource = "login";

        public static readonly string LoginPath = _baseResource + "/Login";
        public static readonly Method LoginMethod = Method.GET;
        public static readonly string UsernameParamName = "username";
        public static readonly string PasswordParamName = "password";
    }
}
