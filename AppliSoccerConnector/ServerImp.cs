using AppliSoccerConnector.AppliSoccerServerConfig;
using AppliSoccerObjects.Modeling;
using AppliSoccerObjects.ResponseObjects;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AppliSoccerConnector
{
    public class ServerImp : IAppServer
    {
        private RestClient _client;
        
        public ServerImp()
        {
            _client = CreateClient();
        }
        private RestClient CreateClient()
        {
            var client = new RestClient(ServerConfig.URL);
            
            // This disable any certificate checking. TODO - add support for using certicate
            client.RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
            return client;
        }
        public async Task<IEnumerable<string>> GetAvailableCountriesAsync()
        {
            var request = new RestRequest(RegistrationConfigs.GetCountriesPath, RegistrationConfigs.GetCountriesMethod);
            var response = await _client.ExecuteAsync<List<string>>(request);
            if (!response.IsSuccessful)
            {
                string errorInfo = "Did not received successfull response for GET_COUNTRIES request. More details: " + response.ErrorMessage +
                    " Inner exception details: " + response.ErrorException.InnerException;
                Console.WriteLine(errorInfo);
                throw new Exception(errorInfo);
            }
            return response.Data;
        }

        public async Task<IEnumerable<TeamDetails>> GetAvailableTeamNames(string country)
        {
            var request = new RestRequest(RegistrationConfigs.GetUnregisteredTeamsPath, RegistrationConfigs.GetUnregisteredTeamsMethod);
            request.AddParameter(RegistrationConfigs.CountryParamName, country);
            var response = await _client.ExecuteAsync<IEnumerable<TeamDetails>>(request);

            if (!response.IsSuccessful)
            {
                string errorInfo = "Did not received successfull response for GET_TEAMS request. More details: " + response.ErrorMessage +
                    " Inner exception details: " + response.ErrorException.InnerException;
                Console.WriteLine(errorInfo);
                throw new Exception(errorInfo);
            }
            return response.Data;
        }

        public async Task<TeamMember> RegisterTeam(string teamId, string adminUsername, string adminPassword)
        {
            var request = new RestRequest(RegistrationConfigs.RegisterTeamPath, RegistrationConfigs.RegisterTeamMethod);
            request.AddQueryParameter(RegistrationConfigs.TeamIdParamName, teamId);
            request.AddQueryParameter(RegistrationConfigs.UsernameParamName, adminUsername);
            request.AddQueryParameter(RegistrationConfigs.PasswordParamName, adminPassword);
            var response = await _client.ExecuteAsync<TeamMember>(request);

            if (!response.IsSuccessful)
            {
                string errorInfo = "Did not received successfull response for REGISTER_TEAM request. More details: " + response.ErrorMessage +
                    " Inner exception details: " + response.ErrorException.InnerException;
                Console.WriteLine(errorInfo);
                throw new Exception(errorInfo);
            }
            return response.Data;
        }
    }
}
