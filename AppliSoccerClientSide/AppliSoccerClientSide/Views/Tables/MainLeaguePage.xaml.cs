using AppliSoccerClientSide.Services;
using AppliSoccerClientSide.ViewModel;
using AppliSoccerClientSide.Views.ViewsUtil.ActionResultUIReporters;
using AppliSoccerObjects.ActionResults.LeagueActions;
using AppliSoccerObjects.Modeling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppliSoccerClientSide.Views.Tables
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainLeaguePage : TabbedPage
    {
        private League _mainLeague;
        public TeamMember MyMember { get; set; }
        public MainLeaguePage()
        {
            InitMyMember();
            InitializeComponent();
        }

        private void InitMyMember()
        {
            MyMember = ApplicationGlobalData.GetMyTeamMember();
        }

        private bool _wasAppeard = false;
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (_wasAppeard)
                return;
            _wasAppeard = true;
            // Pull from server main leauge subLeagues
            await AddMainLeagePages();
        }

        //private async Task PullMainLeagueFromServer()
        //{
        //    GetMainLeagueActionResult actionResult = 
        //        await AppliSoccerServerService.AppServer.GetMainLeague(MyMember.TeamId);
        //    if (actionResult.Status == AppliSoccerObjects.ActionResults.Status.Success)
        //    {
        //        var mainLeague = actionResult.MainLeague;
        //        if (mainLeague == null || mainLeague.Table == null || mainLeague.Table.SubTables.Count == 0)
        //            return;
        //        // For each sub league add tab
        //        List<LeagueTablePage> pages = await Task.Run( () => createLeaguePages(mainLeague));
        //        pages.ForEach(page => Children.Add(page));
        //    }
        //    else
        //    {
        //        ActionResultReporter actionResultReporter = new GetMainLeagueResultReporter();
        //        await actionResultReporter.Report(actionResult, this);
        //        return;
        //    }
        //}

        private async Task AddMainLeagePages()
        {
            List<LeagueTablePage> pages = ApplicationGlobalData.GetLeagueTablePages();
            if (pages != null && pages.Count > 0)
            {
                pages.ForEach(page => Children.Add(page));
            }
            else
            {
                Children.Add(new NoExistLeaguePage());
            }    
        }

        //private void AddTabsLeague(League mainLeague)
        //{
        //    if (mainLeague == null || mainLeague.Table == null) return; 
        //    foreach (var subTable in mainLeague.Table.SubTables)
        //    {
        //        LeagueTableViewModel tableViewModel = LeagueTableViewModel.ConvertToViewModel(subTable);
        //        Children.Add(new LeagueTablePage(tableViewModel));
        //    }
        //}

        //private List<LeagueTablePage> createLeaguePages(League mainLeague)
        //{
        //    List<LeagueTablePage> pages = new List<LeagueTablePage>();
        //    foreach (var subTable in mainLeague.Table.SubTables)
        //    {
        //        LeagueTableViewModel tableViewModel = LeagueTableViewModel.ConvertToViewModel(subTable);
        //        pages.Add(new LeagueTablePage(tableViewModel));
        //    }
        //    return pages;
        //}
    }
}