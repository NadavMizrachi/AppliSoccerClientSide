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
        Task<TeamMember> UpdateTeamMember(TeamMember teamMember);
        Task<bool> CreateUser(User newUser);
        Task<bool> RemoveMember(TeamMember playerToShow);
        Task<bool> CreateOrder(Order order);
    }
}
