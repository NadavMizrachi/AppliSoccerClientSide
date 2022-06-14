using RestSharp; 
using System;
using System.Collections.Generic;
using System.Text;

namespace AppliSoccerConnector.AppliSoccerServerConfig
{
    class TeamMembersConfig
    {
        private static readonly string _baseResource = "teammembers";

        public static readonly string GetTeamMembersPath = _baseResource + "/GetMembers";
        public static readonly Method GetTeamMembersMethod = Method.GET;
        public static readonly string TeamIdParamName = "teamId";
                      
        public static readonly string UpdateTeamMemberPath = _baseResource + "/UpdateTeamMember";
        public static readonly Method UpdateTeamMemberMethod = Method.POST;
        // Team memberreadonly  details inserted as json parameter
                      
        public static readonly string RemoveTeamMemberPath = _baseResource + "/RemoveMember";
        public static readonly Method RemoveTeamMemberMethod = Method.DELETE;
        // Team member details inserted as json parameter


    }
}
