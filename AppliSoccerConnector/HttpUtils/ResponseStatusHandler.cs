using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppliSoccerConnector.HttpUtils
{
    public class ResponseStatusHandler
    {
        public void ThrowExceptionIfNotSucces(IRestResponse response, string requestName)
        {
            if (!response.IsSuccessful)
            {
                string errorInfo = $"Did not received successfull response for ${requestName} request." +
                    $" More details: " + response.ErrorMessage +
                    " Inner exception details: " + response.ErrorException.InnerException;
                Console.WriteLine(errorInfo);
                throw new Exception(errorInfo);
            }
        }
    }
}
