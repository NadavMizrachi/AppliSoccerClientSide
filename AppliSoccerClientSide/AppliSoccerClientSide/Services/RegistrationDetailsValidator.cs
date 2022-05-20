using AppliSoccerObjects.Modeling;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppliSoccerClientSide.Services
{
    public class RegistrationDetailsValidator
    {
        private string _username;
        private string _password;
        private TeamDetails _teamDetails;
        public RegistrationDetailsValidator(string username, string password, TeamDetails teamDetails)
        {
            _username = username;
            _password = password;
            _teamDetails = teamDetails;
        }

        public bool IsValid()
        {
            return IsValidUsername() && IsValidPassword();    
        }

        private bool IsValidUsername()
        {
            return _username != null && _username.Length >= 6;
        }

        private bool IsValidPassword()
        {
            return _password != null && _password.Length >= 6;
        }
    }
}
