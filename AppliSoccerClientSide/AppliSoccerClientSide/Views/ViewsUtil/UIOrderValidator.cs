using AppliSoccerClientSide.Services.Validators;
using AppliSoccerObjects.Modeling;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AppliSoccerClientSide.Views.ViewsUtil
{
    public class UIOrderValidator
    {
        private const string _alertErrorTile = "Order error";
        public static async Task<bool> Validate(Order order, Page page)
        {
            OrderValidator orderValidator = new OrderValidator(order);
            if (!orderValidator.IsValidTitle())
            {
                await page.DisplayAlert(_alertErrorTile, "Title is not valid!", "cancel");
                return false;
            }
            if (!orderValidator.IsValidContent())
            {
                await page.DisplayAlert(_alertErrorTile, "Content is not valid!", "cancel");
                return false;
            }
            if (!orderValidator.IsValidReceivers())
            {
                await page.DisplayAlert(_alertErrorTile, "Receivers list is not valid!", "cancel");
                return false;
            }
            bool isOtherAttributesValid =
                (
                    orderValidator.IsValidGameId() &&
                    orderValidator.IsValidSenderId() &&
                    orderValidator.IsValidSendingDate() &&
                    orderValidator.IsValidTeamId()
                );


            if (!isOtherAttributesValid)
            {
                await page.DisplayAlert(_alertErrorTile, "Internal order error!", "cancel");
                return false;
            }

            // Order is valid
            return true;
        }
    }
}
