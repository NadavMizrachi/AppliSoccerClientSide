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
            return JsonConvert.DeserializeObject<TeamMember>(teamMemberAsJson);
        }
    }
}
