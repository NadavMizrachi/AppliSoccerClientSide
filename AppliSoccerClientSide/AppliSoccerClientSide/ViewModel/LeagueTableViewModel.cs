using AppliSoccerObjects.Modeling;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppliSoccerClientSide.ViewModel
{
    public class LeagueTableViewModel
    {
        public string MainLeagueDescription { get; set; }
        public string SubLeagueDescription { get; set; }
        public string LogoURL { get; set; }
        public List<LeagueTableRowViewModel> Rows { get; set; }


        public static LeagueTableViewModel ConvertToViewModel(SubTable subTable)
        {
            if (subTable == null) return null;
            return new LeagueTableViewModel
            {
                MainLeagueDescription = subTable.Name,
                SubLeagueDescription = subTable.Description,
                Rows = LeagueTableRowViewModel.ConvertToViewModelList(subTable.Rows)
            };
        }
    }
}
