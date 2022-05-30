using AppliSoccerObjects.Modeling;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppliSoccerClientSide.Services
{
    public class UserCreator
    {
        public static User CreateEmptyUserObject(MemberType memberType)
        {
            return new User()
            {
                TeamMember = TeamMemberCreator.CreateEmptyTeamMember(memberType)
            };
        }

        public static void PreparePlayerUserForRegistration(TeamMember AdminMemberDetails, User newUser)
        {
            newUser.IsAdmin = false;
            TeamMemberCreator.PreparePlayerTeamMemberForRegistration(AdminMemberDetails, newUser);
        }
    }
}
