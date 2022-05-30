using RestSharp; 
using System;
using System.Collections.Generic;
using System.Text;

namespace AppliSoccerConnector.AppliSoccerServerConfig
{
    class TeamMembersConfig
    {
        private static string _baseResource = "teammembers";

        public static string GetTeamMembersPath = _baseResource + "/GetMembers";
        public static Method GetCountriesMethod = Method.GET;
        public static string TeamIdParamName = "teamId";
    }   
}
