using AppliSoccerObjects.ActionResults;
using AppliSoccerObjects.ActionResults.EventsActions;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppliSoccerClientSide.Views.ViewsUtil.ActionResultUIReporters
{
    class EventEdittingResultReporter : ActionResultReporter
    {
        protected override string GetReportStringOfActionResult(ActionResult actionResultToReport)
        {
            EditEventActionResult actionResult = actionResultToReport as EditEventActionResult;
            if (actionResult.Status == Status.Success)
            {
                return "Event was created successfully!";
            }

            switch (actionResult.FailReason)
            {
                case EventFailReason.EventExistsOnThisTime:
                    return "Event was not created because there is already event exist on this time.";
                case EventFailReason.EventDataIsUnvalid:
                    return "Event was not created because event data is not valid.";
                case EventFailReason.Unknown:
                default:
                    return "Event was not created because unknown reason.";
            }
        }
    }
}
