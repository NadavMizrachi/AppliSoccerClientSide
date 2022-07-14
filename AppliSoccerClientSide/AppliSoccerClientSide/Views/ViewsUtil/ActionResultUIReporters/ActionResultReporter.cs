using AppliSoccerObjects.ActionResults;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AppliSoccerClientSide.Views.ViewsUtil.ActionResultUIReporters
{
    public abstract class ActionResultReporter
    {
        public async Task Report(ActionResult actionResult, Page page)
        {
            string reportString = GetReportStringOfActionResult(actionResult);
            await page.DisplayAlert("Info", reportString, "ok");
        }

        protected abstract string GetReportStringOfActionResult(ActionResult actionResultToReport);


    }
}
