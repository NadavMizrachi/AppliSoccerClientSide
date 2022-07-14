using AppliSoccerObjects.Modeling;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AppliSoccerClientSide.ViewModel
{
    public class LeagueTableRowViewModel
    {

        public string TeamName { get; set; }
        public string TeamId { get; set; }
        public int Rank { get; set; }
        public int Points { get; set; }
        public int GoalsDiff { get; set; }
        public string Form { get; set; }
        public bool IsMyTeam { get; set; }
        public string TeamLogoUrl { get; set; }
        public FontAttributes FontAttribute
        {
            get
            {
                if (IsMyTeam) return FontAttributes.Bold;
                else return FontAttributes.None;
            }
        }


        public static List<LeagueTableRowViewModel> ConvertToViewModelList(List<TableRow> tableRows)
        {
            if (tableRows == null) return null;
            return tableRows.ConvertAll(row => ConvertToViewModel(row));

        }

        public static LeagueTableRowViewModel ConvertToViewModel(TableRow tableRow)
        {
            if (tableRow == null) return null;
            var myTemMember = ApplicationGlobalData.GetMyTeamMember();
            return new LeagueTableRowViewModel
            {
                Form = tableRow.Form,
                GoalsDiff = tableRow.GoalsDiff,
                Points = tableRow.Points,
                Rank = tableRow.Rank,
                TeamId = tableRow.TeamId,
                TeamName = tableRow.TeamName,
                IsMyTeam = (tableRow.TeamId == myTemMember.TeamId),
                TeamLogoUrl = tableRow.LogoURL
            };
        }
    }
}
