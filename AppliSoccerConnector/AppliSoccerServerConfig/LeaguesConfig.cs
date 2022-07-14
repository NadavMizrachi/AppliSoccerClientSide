using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppliSoccerConnector.AppliSoccerServerConfig
{
    public class LeaguesConfig
    {
        private static readonly string _baseResource = "leagues";

        public static readonly string GetMainLeaguePath = _baseResource + "/GetMainLeague";
        public static readonly Method GetMainLeagueMethod = Method.GET;
        public static readonly string teamIdParamName = "teamId";

    }
}
