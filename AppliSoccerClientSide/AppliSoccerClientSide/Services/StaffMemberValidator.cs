using AppliSoccerObjects.Modeling;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppliSoccerClientSide.Services
{
    public class StaffMemberValidator : TeamMemberValidator
    {
        public StaffMemberValidator(TeamMember teamMember) : base(teamMember)
        {
        }

        public override bool IsValidAdditionalInfo()
        {
            if (!base.IsValidAdditionalInfo())
            {
                return false;
            }
     
            var staffAdditionalInfo = (StaffAdditionalInfo)_teamMember.AdditionalInfo;
            var isCoach = staffAdditionalInfo.IsCoach;
            var managedRoles = staffAdditionalInfo.ManagedRoles;
            
            return (isCoach ||
                   (!isCoach && managedRoles != null && managedRoles.Count > 0));
        }
    }
}
