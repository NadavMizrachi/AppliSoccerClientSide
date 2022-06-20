using System;
using System.Collections.Generic;
using System.Text;

namespace AppliSoccerConnector.Exceptions
{
    class UserNoExistException : Exception
    {
        public UserNoExistException() : base()
        {

        }

        public UserNoExistException(string message) : base(message)
        {

        }
    }
}
