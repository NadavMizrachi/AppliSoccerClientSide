using AppliSoccerObjects.Modeling;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace AppliSoccerClientSide.Services
{
    public class MemberTypeRecognizer
    {
        public static bool IsCoachMember(TeamMember member)
        {
            if (!IsStaff(member))
            {
                return false;
            }
            return member.AdditionalInfo != null ? (member.AdditionalInfo as StaffAdditionalInfo).IsCoach : false;
        }

        public static bool IsAdminMember(TeamMember member)
        {
            if (member == null)
            {
                Debug.WriteLine("Checking member type, but member is null");
                return false;
            }
            return member.MemberType == MemberType.Admin;
        }

        public static bool IsStaff(TeamMember member)
        {
            return member != null && member.MemberType == MemberType.Staff;
        }

        public static bool IsPlayer(TeamMember member)
        {
            return member != null && member.MemberType == MemberType.Player;
        }
    }

    
}
