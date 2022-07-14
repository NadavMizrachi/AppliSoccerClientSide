using AppliSoccerClientSide.Services.Validators;
using AppliSoccerObjects.Modeling;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AppliSoccerClientSide.Views.ViewsUtil
{
    public class UIEventDetailsValidator
    {
        public static async Task<bool> Validate(EventDetails eventDetails, Page page)
        {
            EventDetailsValidator eventValidator = new EventDetailsValidator(eventDetails);
            if (!eventValidator.IsValidTitle())
            {
                await page.DisplayAlert("Error!", "Tile is not valid!", "cancel");
                return false;
            }
            if (!eventValidator.IsValidStartTime())
            {
                await page.DisplayAlert("Error!", "Start time is not valid!", "cancel");
                return false;
            }
            if (!eventValidator.IsValidEndTime())
            {
                await page.DisplayAlert("Error!", "End time is not valid!", "cancel");
                return false;
            }
            if (!eventValidator.IsValidPlace())
            {
                await page.DisplayAlert("Error!", "Place is not valid!", "cancel");
                return false;
            }
            if (!eventValidator.IsValidTimes())
            {
                await page.DisplayAlert("Error!", "Event times are not valid! Notice, event start and end tine nust be in same day.", "cancel");
                return false;
            }
            if (!eventValidator.IsValidDescription())
            {
                await page.DisplayAlert("Error!", "Description is not valid!", "cancel");
                return false;
            }
            bool isOtherAttributesValid =
                (
                    eventValidator.IsValidCreatorId() &&
                    eventValidator.IsValidTeamId() 
                );

            if (!isOtherAttributesValid)
            {
                await page.DisplayAlert("Error!", "Internal event error!", "cancel");
                return false;
            }

            // Event is valid
            return true;
        }
    }
}
