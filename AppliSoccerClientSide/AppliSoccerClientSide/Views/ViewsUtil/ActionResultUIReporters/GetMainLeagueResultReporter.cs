using AppliSoccerObjects.ActionResults;
using AppliSoccerObjects.ActionResults.LeagueActions;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppliSoccerClientSide.Views.ViewsUtil.ActionResultUIReporters
{
    public class GetMainLeagueResultReporter : ActionResultReporter
    {
        protected override string GetReportStringOfActionResult(ActionResult actionResultToReport)
        {
            GetMainLeagueActionResult actionResult = actionResultToReport as GetMainLeagueActionResult;
            if (actionResult.Status == Status.Success)
            {
                return "Event was created successfully!";
            }

            switch (actionResult.FailReason)
            {
                case GetLeagueFailReason.Unknown:
                    return "Cannot pull league data (unknown reason)";
                case GetLeagueFailReason.LeagueNotExist:
                    return "Cannot pull league data (league does not exist)";
                case GetLeagueFailReason.LeagueNotAvailable:
                    return "Cannot pull league data (league data is not available)";
            }
            return "";
        }
    }
}