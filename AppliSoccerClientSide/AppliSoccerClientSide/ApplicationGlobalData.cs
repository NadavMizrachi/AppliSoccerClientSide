using AppliSoccerClientSide.Services;
using AppliSoccerClientSide.ViewModel;
using AppliSoccerClientSide.Views.Tables;
using AppliSoccerConnector.JsonUtils;
using AppliSoccerObjects.ActionResults.LeagueActions;
using AppliSoccerObjects.Modeling;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AppliSoccerClientSide
{
    public class ApplicationGlobalData
    {
        private readonly static IDictionary<string, object> _appProperties 
            = Application.Current.Properties;

        private static TeamMember _myMember;
        private static List<LeagueTablePage> _leagueTablePages;
        // TODO - deserialize once and then cache the object

        public async static Task Init()
        {
            await InitMainLeague();
        }

        private async static Task InitMainLeague()
        {
            TeamMember myMember = ApplicationGlobalData.GetMyTeamMember();
            GetMainLeagueActionResult actionResult =
                await AppliSoccerServerService.AppServer.GetMainLeague(myMember.TeamId);
            if(actionResult == null)
            {
                return;
            }
            if (actionResult.Status == AppliSoccerObjects.ActionResults.Status.Success)
            {
                var mainLeague = actionResult.MainLeague;
                if (mainLeague == null || mainLeague.Table == null || mainLeague.Table.SubTables.Count == 0)
                    return;
                // For each sub league add tab
                _leagueTablePages = createLeaguePages(mainLeague);
            }
        }

        private static List<LeagueTablePage> createLeaguePages(League mainLeague)
        {
            List<LeagueTablePage> pages = new List<LeagueTablePage>();
            foreach (var subTable in mainLeague.Table.SubTables)
            {
                LeagueTableViewModel tableViewModel = LeagueTableViewModel.ConvertToViewModel(subTable);
                pages.Add(new LeagueTablePage(tableViewModel));
            }
            return pages;
        }

        public static void InsertTeamMember(TeamMember teamMember)
        {
            _myMember = teamMember;
        }

        public static TeamMember GetMyTeamMember()
        {
            return _myMember;
        }

        public static List<LeagueTablePage> GetLeagueTablePages()
        {
            return _leagueTablePages;
        }

        public static void CleanData()
        {
            _leagueTablePages = null;
            _myMember = null;
        }

    }
}
