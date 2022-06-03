using AppliSoccerObjects.Modeling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AppliSoccerClientSide.Services
{
    public class TeamMemberCreator
    {
        public static TeamMember CopyTeamMember(TeamMember memberToCopy)
        {
            return new TeamMember()
            {
                TeamId = memberToCopy.TeamId,
                PhoneNumber = memberToCopy.PhoneNumber,
                ID = memberToCopy.ID,
                Description = memberToCopy.Description,
                BirthDate = memberToCopy.BirthDate,
                AdditionalInfo = CopyAdditionalInfo(memberToCopy),
                FirstName = memberToCopy.FirstName,
                LastName = memberToCopy.LastName,
                MemberType = memberToCopy.MemberType,
                TeamName = memberToCopy.TeamName
            };
        }

        internal static void PrepareTeamMemberForRegistration(TeamMember AdminMemberDetails, User newUser)
        {
            newUser.TeamMember.TeamId = AdminMemberDetails.TeamId;
            newUser.TeamMember.TeamName = AdminMemberDetails.TeamName;
            newUser.TeamMember.ID = newUser.Username;
        }

        public static Object CopyAdditionalInfo(TeamMember memberToCopy)
        {
            if(memberToCopy.AdditionalInfo == null)
            {
                return null;
            }

            switch (memberToCopy.MemberType)
            {
                case MemberType.Player:
                    {
                        return CopyPlayerAdditionalInfo((PlayerAdditionalInfo)memberToCopy.AdditionalInfo);
                    }
                case MemberType.Staff:
                    {
                        return CopyStaffAddiotionalInfo((StaffAdditionalInfo)memberToCopy.AdditionalInfo);
                    }
                default: return null;
            }
        }

        private static PlayerAdditionalInfo  CopyPlayerAdditionalInfo(PlayerAdditionalInfo info)
        {
            return new PlayerAdditionalInfo()
            {
                Number = info.Number,
                Role = info.Role
            };
        }

        private static StaffAdditionalInfo CopyStaffAddiotionalInfo(StaffAdditionalInfo info)
        {
            return new StaffAdditionalInfo()
            {
                IsCoach = info.IsCoach,
                ManagedRoles = info.ManagedRoles.ToList()
            };
        }

        internal static TeamMember CreateEmptyTeamMember(MemberType memberType)
        {
            switch (memberType)
            {
                case MemberType.Player:
                    {
                        return CreateEmptyPlayerObject();
                    }
                case MemberType.Staff:
                    {
                        return CreateEmptyStaffObject();
                    }
                default: return CreateEmptyPlayerObject();
            }
        }

        public static TeamMember CreateEmptyPlayerObject()
        {
            return new TeamMember()
            {
                MemberType = MemberType.Player,
                AdditionalInfo = new PlayerAdditionalInfo()
            };
        }

        public static TeamMember CreateEmptyStaffObject()
        {
            return new TeamMember()
            {
                MemberType = MemberType.Staff,
                AdditionalInfo = new StaffAdditionalInfo()
            };
        }

    }
}
