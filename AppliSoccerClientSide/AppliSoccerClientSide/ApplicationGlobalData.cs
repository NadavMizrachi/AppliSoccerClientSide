using AppliSoccerConnector.JsonUtils;
using AppliSoccerObjects.Modeling;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Xamarin.Forms;

namespace AppliSoccerClientSide
{
    public class ApplicationGlobalData
    {
        private readonly static IDictionary<string, object> _appProperties 
            = Application.Current.Properties;

        // TODO - deserialize once and then cache the object
        public static void Insert(TeamMember teamMember)
        {
            var objAsJson = JsonConvert.SerializeObject(teamMember);
            Application.Current.Properties[AppPropertiesConsts.TeamMemberKey] = objAsJson;
        }

        public static TeamMember GetMyTeamMember()
        {
            if (!_appProperties.ContainsKey(AppPropertiesConsts.TeamMemberKey))
            {
                throw new KeyNotFoundException();
            }
            string teamMemberAsJson = _appProperties[AppPropertiesConsts.TeamMemberKey].ToString();
            Debug.WriteLine("Team member as json : " + teamMemberAsJson);
            TeamMember teamMember = JsonConvert.DeserializeObject<TeamMember>(teamMemberAsJson);
            object additionalInfo = TeamMemberDeserialization.DeserializeAdditionalInfo(teamMember);
            teamMember.AdditionalInfo = additionalInfo;
            return teamMember;
        }
    }
}
