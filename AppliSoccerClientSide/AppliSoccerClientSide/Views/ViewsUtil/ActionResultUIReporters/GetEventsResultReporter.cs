using AppliSoccerObjects.ActionResults;
using AppliSoccerObjects.ActionResults.EventsActions;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppliSoccerClientSide.Views.ViewsUtil.ActionResultUIReporters
{
    public class GetEventsResultReporter : ActionResultReporter
    {
        protected override string GetReportStringOfActionResult(ActionResult actionResultToReport)
        {
            GetEventsActionResult actionResult = actionResultToReport as GetEventsActionResult;
            if(actionResult.Status == Status.Success)
            {
                return "Successfully got events from server!";
            }

            switch (actionResult.FailReason)
            {
                case GetEventFailReason.Unknown:
                default:
                    return "Can't get events from server. Unknown failure reason.";
            }
        }
    }
}