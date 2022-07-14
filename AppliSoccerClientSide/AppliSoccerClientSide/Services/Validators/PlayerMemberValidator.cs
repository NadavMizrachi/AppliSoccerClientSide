using AppliSoccerObjects.Modeling;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppliSoccerClientSide.Services.Validators
{
    public class PlayerMemberValidator : TeamMemberValidator
    {
        public PlayerMemberValidator(TeamMember teamMember) : base(teamMember)
        {
        }

        public override bool IsValidAdditionalInfo()
        {
            return base.IsValidAdditionalInfo();
            
            var playerAdditionalInfo = (PlayerAdditionalInfo)_teamMember.AdditionalInfo;
            var number = playerAdditionalInfo.Number;
            var role = playerAdditionalInfo.Role;

            return number > 0;
        }
    }
}
