using AppliSoccerObjects.Modeling;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AppliSoccerConnector
{
    public interface IAppServer
    {
        Task<TeamMember> Login(string username, string password); 
        Task<IEnumerable<string>> GetAvailableCountriesAsync();
        Task<IEnumerable<TeamDetails>> GetAvailableTeamNames(string country);
        Task<TeamMember> RegisterTeam(string teamId, string adminUsername, string adminPassword);
        Task<List<TeamMember>> PullTeamMembers(string teamId);
        Task<List<OrderMetadata>> PullNewSenderOrders(DateTime lowerBoundDate, string askerId);
        Task<TeamMember> UpdateTeamMember(TeamMember teamMember);
        Task<bool> CreateUser(User newUser);
        Task<bool> RemoveMember(TeamMember playerToShow);
        Task<List<OrderMetadata>> FetchOrdersMetadataForSender(DateTime upperBoundDate, int quantity, string askerId);
        Task<bool> CreateOrder(Order order);
        Task<List<OrderMetadata>> FetchOrdersMetadata(DateTime earliestOrderDate, int batchSize, String receiverId);
        Task<List<OrderMetadata>> PullNewOrders(DateTime latestOrderDate, string receiverId);
        Task<OrderPayload> GetOrderPayload(string orderId, string askerId);
        Task<SentOrderWithReceiversInfo> GetSentOrder(string id);
    }
}
