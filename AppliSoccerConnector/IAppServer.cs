﻿using AppliSoccerObjects.Modeling;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AppliSoccerConnector
{
    public interface IAppServer
    {
        Task<IEnumerable<string>> GetAvailableCountriesAsync();
        Task<IEnumerable<TeamDetails>> GetAvailableTeamNames(string country);
        Task<TeamMember> RegisterTeam(string teamId, string adminUsername, string adminPassword);
        Task<List<TeamMember>> PullTeamMembers(string teamId);
        Task<bool> UpdateTeamMember(TeamMember teamMember);
        Task<bool> CreateUser(User newUser);
    }
}
