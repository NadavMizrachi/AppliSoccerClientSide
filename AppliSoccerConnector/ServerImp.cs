using AppliSoccerConnector.AppliSoccerServerConfig;
using AppliSoccerObjects.Modeling;
using AppliSoccerObjects.ResponseObjects;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        public async Task<List<TeamMember>> PullTeamMembers(string teamId)
        {
            var request = new RestRequest(TeamMembersConfig.GetTeamMembersPath, TeamMembersConfig.GetCountriesMethod);
            request.AddQueryParameter(TeamMembersConfig.TeamIdParamName, teamId);
            var response = await _client.ExecuteAsync<List<TeamMember>>(request);

            if (!response.IsSuccessful)
            {
                string errorInfo = "Did not received successfull response for PullTemMembers request. More details: " + response.ErrorMessage +
                    " Inner exception details: " + response.ErrorException.InnerException;
                Console.WriteLine(errorInfo);
                throw new Exception(errorInfo);
            }

            foreach (var member in response.Data)
            {
                if (member.AdditionalInfo != null)
                {
                    object additionalInfo = DeserializeAdditionalInfo(member);
                    member.AdditionalInfo = additionalInfo;
                }
            }
            return response.Data;
        }

        private object DeserializeAdditionalInfo(TeamMember member)
        {
            string additionalInfoAsJson = JsonConvert.SerializeObject(member.AdditionalInfo).ToString();
            Debug.WriteLine("Additional Info of " + member.FirstName + " " + member.LastName + " " + additionalInfoAsJson);
            if(member.MemberType == MemberType.Player)
            {
                return JsonConvert.DeserializeObject<PlayerAdditionalInfo>(additionalInfoAsJson);
            }
            else if(member.MemberType == MemberType.Staff)
            {
                return JsonConvert.DeserializeObject<PlayerAdditionalInfo>(additionalInfoAsJson);
            }
            return null;
        }


        public Task<bool> UpdateTeamMember(TeamMember teamMember)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> CreateUser(User newUser)
        {
            var request = new RestRequest(RegistrationConfigs.RegisterUserPath, RegistrationConfigs.RegisterUserMethod);
            request.AddHeader("Accept", "application/json");
            request.AddJsonBody(newUser);
            var response = await _client.ExecuteAsync<bool>(request);
            if(!response.IsSuccessful)
            {
                Debug.WriteLine("The request of CreateUser was not success");
                string errorInfo = "Did not received successfull response for CreateUser request. More details: " + response.ErrorMessage +
                    " Inner exception details: " + response.ErrorException.InnerException;
                Console.WriteLine(errorInfo);
                throw new Exception(errorInfo);
            }
            bool isCreationSucceed = response.Data;
            if (!isCreationSucceed)
            {
                Debug.WriteLine("Got response for CreateUser, but the creation failed");
            }
            return isCreationSucceed;
        }

        public async Task<TeamMember> Login(string username, string password)
        {
            var request = new RestRequest(LoginConfig.LoginPath, LoginConfig.LoginMethod);
            request.AddQueryParameter(LoginConfig.UsernameParamName, username);
            request.AddQueryParameter(LoginConfig.PasswordParamName, password);
            var response = await _client.ExecuteAsync<TeamMember>(request);
            
            if (!response.IsSuccessful)
            {
                Debug.WriteLine("The request of Login was not success");
                string errorInfo = "Did not received successfull response for Login request. More details: " + response.ErrorMessage +
                    " Inner exception details: " + response.ErrorException.InnerException;
                Console.WriteLine(errorInfo);
                throw new Exception(errorInfo);
            }
            TeamMember teamMember = response.Data;
            if (teamMember == null)
            {
                Debug.WriteLine("Login faild");
            }
            return teamMember;
        }
    }
}
