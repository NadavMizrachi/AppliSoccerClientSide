using AppliSoccerObjects.Modeling;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace AppliSoccerConnector.JsonUtils
{
    public class TeamMemberDeserialization
    {
        public static object DeserializeAdditionalInfo(TeamMember member)
        {
            string additionalInfoAsJson = JsonConvert.SerializeObject(member.AdditionalInfo).ToString();
            Debug.WriteLine("Additional Info of " + member.FirstName + " " + member.LastName + " " + additionalInfoAsJson);
            if (member.MemberType == MemberType.Player)
            {
                return JsonConvert.DeserializeObject<PlayerAdditionalInfo>(additionalInfoAsJson);
            }
            else if (member.MemberType == MemberType.Staff)
            {
                return JsonConvert.DeserializeObject<StaffAdditionalInfo>(additionalInfoAsJson);
            }
            return null;
        }
    }
}
