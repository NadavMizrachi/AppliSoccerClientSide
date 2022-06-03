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
        public static Method GetTeamMembersMethod = Method.GET;
        public static string TeamIdParamName = "teamId";

        public static string UpdateTeamMemberPath = _baseResource + "/UpdateTeamMember";
        public static Method UpdateTeamMemberMethod = Method.POST;
        // Team member details inserted as json parameter

        public static string RemoveTeamMemberPath = _baseResource + "/RemoveMember";
        public static Method RemoveTeamMemberMethod = Method.DELETE;
        // Team member details inserted as json parameter


    }
}
