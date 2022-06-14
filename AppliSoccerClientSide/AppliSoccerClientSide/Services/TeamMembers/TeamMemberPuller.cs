using AppliSoccerObjects.Modeling;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppliSoccerClientSide.Services.TeamMembers
{
    public class TeamMemberPuller
    {
        public static Task Pull(ObservableCollection<TeamMember> OutCollection, MemberType type,TeamMember asker)
        {
            return Task.Run(() =>
            {
                foreach (var member in OutCollection)
                {
                    // Clean last collection content
                    OutCollection.Remove(member);
                }
                var teamMembers = AppliSoccerServerService.AppServer.PullTeamMembers(asker.TeamId).Result;
                var membersFromServer =
                    teamMembers.Where(teamMember => teamMember.MemberType == type && !teamMember.ID.Equals(asker.ID)).ToList();
                membersFromServer.ForEach(member => OutCollection.Add(member));
            });
        }
    }
}
