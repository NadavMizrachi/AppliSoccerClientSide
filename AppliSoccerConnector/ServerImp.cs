using AppliSoccerConnector.AppliSoccerServerConfig;
using AppliSoccerConnector.HttpUtils;
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
using System.Reflection;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using AppliSoccerConnector.JsonUtils;
using AppliSoccerConnector.Exceptions;

namespace AppliSoccerConnector
{
    public class ServerImp : IAppServer
    {
        private RestClient _client;
        private ResponseStatusHandler _responseStatusHandler;
        public ServerImp()
        {
            _client = CreateClient();
            _responseStatusHandler = new ResponseStatusHandler();
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

            var functionName = GetCurrentMethod();
            _responseStatusHandler.ThrowExceptionIfNotSucces(response, functionName);

            return response.Data;
        }

        public async Task<IEnumerable<TeamDetails>> GetAvailableTeamNames(string country)
        {
            var request = new RestRequest(RegistrationConfigs.GetUnregisteredTeamsPath, RegistrationConfigs.GetUnregisteredTeamsMethod);
            request.AddParameter(RegistrationConfigs.CountryParamName, country);
            var response = await _client.ExecuteAsync<IEnumerable<TeamDetails>>(request);

            var functionName = GetCurrentMethod();
            _responseStatusHandler.ThrowExceptionIfNotSucces(response, functionName);

            return response.Data;
        }

        public async Task<TeamMember> RegisterTeam(string teamId, string adminUsername, string adminPassword)
        {
            var request = new RestRequest(RegistrationConfigs.RegisterTeamPath, RegistrationConfigs.RegisterTeamMethod);
            request.AddQueryParameter(RegistrationConfigs.TeamIdParamName, teamId);
            request.AddQueryParameter(RegistrationConfigs.UsernameParamName, adminUsername);
            request.AddQueryParameter(RegistrationConfigs.PasswordParamName, adminPassword);
            var response = await _client.ExecuteAsync<TeamMember>(request);

            var functionName = GetCurrentMethod();
            _responseStatusHandler.ThrowExceptionIfNotSucces(response, functionName);

            return response.Data;
        }

        public async Task<List<TeamMember>> PullTeamMembers(string teamId)
        {
            var request = new RestRequest(TeamMembersConfig.GetTeamMembersPath, TeamMembersConfig.GetTeamMembersMethod);
            request.AddQueryParameter(TeamMembersConfig.TeamIdParamName, teamId);
            var response = await _client.ExecuteAsync<List<TeamMember>>(request);

            var functionName = GetCurrentMethod();
            _responseStatusHandler.ThrowExceptionIfNotSucces(response, functionName);

            foreach (var member in response.Data)
            {
                if (member.AdditionalInfo != null)
                {
                    object additionalInfo = TeamMemberDeserialization.DeserializeAdditionalInfo(member);
                    member.AdditionalInfo = additionalInfo;
                }
            }
            return response.Data;
        }


        public async Task<TeamMember> UpdateTeamMember(TeamMember teamMemberNewDetails)
        {
            var request = new RestRequest(TeamMembersConfig.UpdateTeamMemberPath, TeamMembersConfig.UpdateTeamMemberMethod);
            request.AddHeader("Accept", "application/json");
            request.AddJsonBody(teamMemberNewDetails);
            var response = await _client.ExecuteAsync<TeamMember>(request);

            var functionName = GetCurrentMethod();
            _responseStatusHandler.ThrowExceptionIfNotSucces(response, functionName);

            return response.Data;
        }

        public async Task<bool> CreateUser(User newUser)
        {
            var request = new RestRequest(RegistrationConfigs.RegisterUserPath, RegistrationConfigs.RegisterUserMethod);
            request.AddHeader("Accept", "application/json");
            request.AddJsonBody(newUser);
            var response = await _client.ExecuteAsync<bool>(request);

            var functionName = GetCurrentMethod();
            _responseStatusHandler.ThrowExceptionIfNotSucces(response, functionName);

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

            var functionName = GetCurrentMethod();
            _responseStatusHandler.ThrowExceptionIfNotSucces(response, functionName);
            TeamMember teamMember = response.Data;
            if (teamMember == null)
                return null;

            if (teamMember.AdditionalInfo != null)
            {
                object additionalInfo = TeamMemberDeserialization.DeserializeAdditionalInfo(teamMember);
                teamMember.AdditionalInfo = additionalInfo;
            }

            if (teamMember == null)
            {
                Debug.WriteLine("Login faild");
            }
            return teamMember;
        }

        public async Task<bool> RemoveMember(TeamMember memberToRemove)
        {
            var request = new RestRequest(TeamMembersConfig.RemoveTeamMemberPath, TeamMembersConfig.RemoveTeamMemberMethod);
            request.AddHeader("Accept", "application/json");
            request.AddJsonBody(memberToRemove);
            var response = await _client.ExecuteAsync<bool>(request);

            var functionName = GetCurrentMethod();
            _responseStatusHandler.ThrowExceptionIfNotSucces(response, functionName);

            return response.Data;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public string GetCurrentMethod()
        {
            var st = new StackTrace();
            var sf = st.GetFrame(1);

            return sf.GetMethod().Name;
        }

        public async Task<bool> CreateOrder(Order order)
        {
            var request = new RestRequest(OrderConfig.CreateOrderPath, OrderConfig.CreateOrderMethod);
            request.AddHeader("Accept", "application/json");
            request.AddJsonBody(order);
            Debug.WriteLine(JsonConvert.SerializeObject(order).ToString());
            var response = await _client.ExecuteAsync<bool>(request);

            var functionName = GetCurrentMethod();
            _responseStatusHandler.ThrowExceptionIfNotSucces(response, functionName);

            return response.Data;
        }

        public async Task<List<OrderMetadata>> FetchOrdersMetadata(DateTime earliestOrderDate, int batchSize, String receiverId)
        {

            var request = new RestRequest(OrderConfig.FetchOrdersMetadataPath, OrderConfig.FetchOrdersMetadataMethod);
            request.AddQueryParameter(OrderConfig.upperBoundDateParamName, DateStringFormatter.Format(TimeZoneInfo.ConvertTimeToUtc(earliestOrderDate)));
            request.AddQueryParameter(OrderConfig.ordersQuantityParamName, batchSize.ToString());
            request.AddQueryParameter(OrderConfig.receiverIdParamName, receiverId);
            var response = await _client.ExecuteAsync<List<OrderMetadata>>(request);
            Debug.WriteLine(" *** FetchOrdersMetadata *** Got response from server");
            var functionName = GetCurrentMethod();
            _responseStatusHandler.ThrowExceptionIfNotSucces(response, functionName);

            return response.Data;
        }

        public async Task<List<OrderMetadata>> PullNewOrders(DateTime latestOrderDate, string receiverId)
        {
            var request = new RestRequest(OrderConfig.PullNewOrdersMetadataPath, OrderConfig.PullNewOrdersMetadataMethod);
            request.AddQueryParameter(OrderConfig.lowerBoundDateParamName, DateStringFormatter.Format(TimeZoneInfo.ConvertTimeToUtc(latestOrderDate)));
            request.AddQueryParameter(OrderConfig.ordersQuantityParamName, int.MaxValue.ToString());
            request.AddQueryParameter(OrderConfig.receiverIdParamName, receiverId);
            var response = await _client.ExecuteAsync<List<OrderMetadata>>(request);
            Debug.WriteLine(" *** PullNewOrders *** Got response from server");
            var functionName = GetCurrentMethod();
            _responseStatusHandler.ThrowExceptionIfNotSucces(response, functionName);

            return response.Data;
        }

        public async Task<OrderPayload> GetOrderPayload(string orderId, string askerId)
        {
            var request = new RestRequest(OrderConfig.GetOrderPayloadPath, OrderConfig.GetOrderPayloadMethod);
            request.AddQueryParameter(OrderConfig.orderIdParamName, orderId);
            request.AddQueryParameter(OrderConfig.askerIdParamName, askerId);
            
            var response = await _client.ExecuteAsync<OrderPayload>(request);

            var functionName = GetCurrentMethod();
            _responseStatusHandler.ThrowExceptionIfNotSucces(response, functionName);

            return response.Data;
        }
    }
}
