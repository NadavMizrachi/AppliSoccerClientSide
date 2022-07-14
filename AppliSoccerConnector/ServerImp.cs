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
using RestSharp.Serializers.NewtonsoftJson;
using AppliSoccerObjects.ActionResults.EventsActions;
using AppliSoccerObjects.ActionResults.LeagueActions;

namespace AppliSoccerConnector
{
    public class ServerImp : IAppServer
    {
        private RestClient _client;
        private ResponseStatusHandler _responseStatusHandler;
        public ServerImp()
        {
            _client = CreateClient();
            _client.UseNewtonsoftJson();
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
            try
            {
                var request = new RestRequest(RegistrationConfigs.GetCountriesPath, RegistrationConfigs.GetCountriesMethod);
                var response = await _client.ExecuteAsync<List<string>>(request);
                return response.Data;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception has occurred in GetAvailableCountriesAsync. Exception details: " + ex.Message);
                return new List<String>();
            }
        }

        public async Task<IEnumerable<TeamDetails>> GetAvailableTeamNames(string country)
        {
            try
            {
                var request = new RestRequest(RegistrationConfigs.GetUnregisteredTeamsPath, RegistrationConfigs.GetUnregisteredTeamsMethod);
                request.AddParameter(RegistrationConfigs.CountryParamName, country);
                var response = await _client.ExecuteAsync<IEnumerable<TeamDetails>>(request);
                return response.Data;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception has occurred in GetAvailableTeamNames. Exception details: " + ex.Message);
                return new List<TeamDetails>();
            }
        }

        public async Task<TeamMember> RegisterTeam(string teamId, string adminUsername, string adminPassword)
        {
            try
            {
                var request = new RestRequest(RegistrationConfigs.RegisterTeamPath, RegistrationConfigs.RegisterTeamMethod);
                request.AddQueryParameter(RegistrationConfigs.TeamIdParamName, teamId);
                request.AddQueryParameter(RegistrationConfigs.UsernameParamName, adminUsername);
                request.AddQueryParameter(RegistrationConfigs.PasswordParamName, adminPassword);
                var response = await _client.ExecuteAsync<TeamMember>(request);
                return response.Data;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception has occurred in RegisterTeam. Exception details: " + ex.Message);
                return null;
            }
        }

        public async Task<List<TeamMember>> PullTeamMembers(string teamId)
        {
            try
            {
                var request = new RestRequest(TeamMembersConfig.GetTeamMembersPath, TeamMembersConfig.GetTeamMembersMethod);
                request.AddQueryParameter(TeamMembersConfig.TeamIdParamName, teamId);
                var response = await _client.ExecuteAsync<List<TeamMember>>(request);
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
            catch (Exception ex)
            {
                Console.WriteLine("Exception has occurred in PullTeamMembers. Exception details: " + ex.Message);
                return new List<TeamMember>();
            }
        }


        public async Task<TeamMember> UpdateTeamMember(TeamMember teamMemberNewDetails)
        {
            try
            {
                var request = new RestRequest(TeamMembersConfig.UpdateTeamMemberPath, TeamMembersConfig.UpdateTeamMemberMethod);
                request.AddHeader("Accept", "application/json");
                request.AddJsonBody(teamMemberNewDetails);
                var response = await _client.ExecuteAsync<TeamMember>(request);
                return response.Data;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception has occurred in UpdateTeamMember. Exception details: " + ex.Message);
                return null;
            }
        }

        public async Task<bool> CreateUser(User newUser)
        {
            try
            {
                var request = new RestRequest(RegistrationConfigs.RegisterUserPath, RegistrationConfigs.RegisterUserMethod);
                request.AddHeader("Accept", "application/json");
                request.AddJsonBody(newUser);
                var response = await _client.ExecuteAsync<bool>(request);
                bool isCreationSucceed = response.Data;
                if (!isCreationSucceed)
                {
                    Debug.WriteLine("Got response for CreateUser, but the creation failed");
                }
                return isCreationSucceed;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception has occurred in CreateUser. Exception details: " + ex.Message);
                return false;
            }
        }

        public async Task<TeamMember> Login(string username, string password)
        {
            try
            {
                var request = new RestRequest(LoginConfig.LoginPath, LoginConfig.LoginMethod);
                request.AddQueryParameter(LoginConfig.UsernameParamName, username);
                request.AddQueryParameter(LoginConfig.PasswordParamName, password);
                var response = await _client.ExecuteAsync<TeamMember>(request);
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
            catch (Exception ex) 
            { 
                Console.WriteLine("Exception has occurred in Login. Exception details: " + ex.Message);
                return null; 
            }
        }

        public async Task<bool> RemoveMember(TeamMember memberToRemove)
        {
            try
            {
                var request = new RestRequest(TeamMembersConfig.RemoveTeamMemberPath, TeamMembersConfig.RemoveTeamMemberMethod);
                request.AddHeader("Accept", "application/json");
                request.AddJsonBody(memberToRemove);
                var response = await _client.ExecuteAsync<bool>(request);
                return response.Data;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception has occurred in RemoveMember. Exception details: " + ex.Message);
                return false;
            }
        }

        public async Task<bool> CreateOrder(Order order)
        {
            try
            {
                var request = new RestRequest(OrderConfig.CreateOrderPath, OrderConfig.CreateOrderMethod);
                request.AddHeader("Accept", "application/json");
                request.AddJsonBody(order);
                Debug.WriteLine(JsonConvert.SerializeObject(order).ToString());
                var response = await _client.ExecuteAsync<bool>(request);
                return response.Data;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception has occurred in CreateOrder. Exception details: " + ex.Message);
                return false;
            }
        }

        public async Task<List<OrderMetadata>> FetchOrdersMetadata(DateTime earliestOrderDate, int batchSize, String receiverId)
        {
            try
            {
                var request = new RestRequest(OrderConfig.FetchOrdersMetadataPath, OrderConfig.FetchOrdersMetadataMethod);
                request.AddQueryParameter(OrderConfig.upperBoundDateParamName, DateStringFormatter.Format(earliestOrderDate));
                request.AddQueryParameter(OrderConfig.ordersQuantityParamName, batchSize.ToString());
                request.AddQueryParameter(OrderConfig.receiverIdParamName, receiverId);
                var response = await _client.ExecuteAsync<List<OrderMetadata>>(request);
                return response.Data;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception has occurred in FetchOrdersMetadata. Exception details: " + ex.Message);
                return new List<OrderMetadata>();
            }
        }

        public async Task<List<OrderMetadata>> PullNewOrders(DateTime latestOrderDate, string receiverId)
        {
            try
            {
                var request = new RestRequest(OrderConfig.PullNewOrdersMetadataPath, OrderConfig.PullNewOrdersMetadataMethod);
                request.AddQueryParameter(OrderConfig.lowerBoundDateParamName, DateStringFormatter.Format(latestOrderDate));
                request.AddQueryParameter(OrderConfig.ordersQuantityParamName, int.MaxValue.ToString());
                request.AddQueryParameter(OrderConfig.receiverIdParamName, receiverId);
                var response = await _client.ExecuteAsync<List<OrderMetadata>>(request);
                return response.Data;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception has occurred in PullNewOrders. Exception details: " + ex.Message);
                return new List<OrderMetadata>();
            }
        }

        public async Task<OrderPayload> GetOrderPayload(string orderId, string askerId)
        {
            try
            {
                var request = new RestRequest(OrderConfig.GetOrderPayloadPath, OrderConfig.GetOrderPayloadMethod);
                request.AddQueryParameter(OrderConfig.orderIdParamName, orderId);
                request.AddQueryParameter(OrderConfig.askerIdParamName, askerId);
                var response = await _client.ExecuteAsync<OrderPayload>(request);
                return response.Data;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception has occurred in GetOrderPayload. Exception details: " + ex.Message);
                return null;
            }
        }

        public async Task<List<OrderMetadata>> PullNewSenderOrders(DateTime lowerBoundDate, string askerId)
        {
            try
            {
                var request = new RestRequest(OrderConfig.PullNewOrdersMetadataForSenderPath, OrderConfig.PullNewOrdersMetadataForSenderMethod);
                request.AddQueryParameter(OrderConfig.lowerBoundDateParamName, DateStringFormatter.Format(lowerBoundDate));
                request.AddQueryParameter(OrderConfig.ordersQuantityParamName, int.MaxValue.ToString());
                request.AddQueryParameter(OrderConfig.senderIdParamName, askerId);
                var response = await _client.ExecuteAsync<List<OrderMetadata>>(request);
                return response.Data;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception has occurred in PullNewSenderOrders. Exception details: " + ex.Message);
                return new List<OrderMetadata>();
            }
        }

        public async Task<List<OrderMetadata>> FetchOrdersMetadataForSender(DateTime upperBoundDate, int quantity, string askerId)
        {
            try
            {
                var request = new RestRequest(OrderConfig.FetchOrdersMetadataForSenderPath, OrderConfig.FetchOrdersMetadataForSenderMethod);
                request.AddQueryParameter(OrderConfig.upperBoundDateParamName, DateStringFormatter.Format(upperBoundDate));
                request.AddQueryParameter(OrderConfig.ordersQuantityParamName, quantity.ToString());
                request.AddQueryParameter(OrderConfig.senderIdParamName, askerId);
                var response = await _client.ExecuteAsync<List<OrderMetadata>>(request);
                List<OrderMetadata> output = response.Data;
                return output;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception has occurred in FetchOrdersMetadataForSender. Exception details: " + ex.Message);
                return new List<OrderMetadata>();
            }
        }

        public async Task<SentOrderWithReceiversInfo> GetSentOrder(string id)
        {
            try
            {
                var request = new RestRequest(OrderConfig.GetOrderPath, OrderConfig.GetOrderMethod);
                request.AddQueryParameter(OrderConfig.orderIdParamName, id);
                var response = await _client.ExecuteAsync<SentOrderWithReceiversInfo>(request);
                return response.Data;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception has occurred in GetSentOrder. Exception details: " + ex.Message);
                return null;
            }
        }

        public async Task<CreateEventActionResult> CreateEvent(EventDetails eventDetails)
        {
            try
            {
                var request = new RestRequest(EventsConfig.CreateEventPath, EventsConfig.CreateEventMethod);
                request.AddJsonBody(eventDetails);
                var response = await _client.ExecuteAsync<CreateEventActionResult>(request);
                return response.Data;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception has occurred in CreateEvent. Exception details: " + ex.Message);
                return null;
            }
        }

        public async Task<GetEventsActionResult> GetEvents(DateTime lowerBoundDate, DateTime upBoundDate, string askerId)
        {
            try
            {
                var request = new RestRequest(EventsConfig.GetEventsPath, EventsConfig.GetEventsMethod);
                request.AddQueryParameter(EventsConfig.lowerBoundDateParamName, DateStringFormatter.Format(lowerBoundDate));
                request.AddQueryParameter(EventsConfig.upBoundDateParamName, DateStringFormatter.Format(upBoundDate));
                request.AddQueryParameter(EventsConfig.GetEventsAskerIdParamName, askerId);
                var response = await _client.ExecuteAsync<GetEventsActionResult>(request);
                return response.Data;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception has occurred in GetEvents. Exception details: " + ex.Message);
                return null;
            }
        }

        public Task<GetEventsActionResult> GetAllEvents(string askerId)
        {
            return GetEvents(DateTime.MinValue, DateTime.MaxValue, askerId);
        }

        public async Task<EditEventActionResult> EditEvent(EventDetails edittedEvent)
        {
            try
            {
                var request = new RestRequest(EventsConfig.EditEventPath, EventsConfig.EditEventMethod);
                request.AddJsonBody(edittedEvent);
                var response = await _client.ExecuteAsync<EditEventActionResult>(request);
                return response.Data;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception has occurred in EditEvent. Exception details: " + ex.Message);
                return null;
            }
        }

        public async Task<GetMainLeagueActionResult> GetMainLeague(string teamId)
        {
            try
            {
                var request = new RestRequest(LeaguesConfig.GetMainLeaguePath, LeaguesConfig.GetMainLeagueMethod);
                request.AddQueryParameter(LeaguesConfig.teamIdParamName, teamId);
                var response = await _client.ExecuteAsync<GetMainLeagueActionResult>(request);
                return response.Data;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception has occurred in GetMainLeague. Exception details: " + ex.Message);
                return null;
            }
        }
    }
}
