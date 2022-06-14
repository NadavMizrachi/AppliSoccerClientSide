using AppliSoccerObjects.Modeling;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppliSoccerClientSide.ViewModel
{
    public class ReceiverViewModel
    {
        public String Fullname { get; set; }
        public String MemberId { get; set; }
        public bool IsShouldReceive { get; set; }

        public static ReceiverViewModel FromTeamMember(TeamMember teamMember)
        {
            return new ReceiverViewModel()
            {
                Fullname = teamMember.FirstName + " " + teamMember.LastName,
                MemberId = teamMember.ID,
                IsShouldReceive = false
            };
        }
    }
}
