using System;
using System.Collections.Generic;
using System.Text;

namespace AppliSoccerClientSide.Exceptions
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
