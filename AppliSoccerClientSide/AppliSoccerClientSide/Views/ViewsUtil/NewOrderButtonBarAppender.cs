using AppliSoccerClientSide.Views.Orders;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AppliSoccerClientSide.Views.ViewsUtil
{
    public class NewOrderButtonBarAppender
    {
        /// <summary>
        /// Append the New Order tool bar item to the given page. The event that will be fired
        /// when clicking the button is navigate to NewOrderPage
        /// </summary>
        /// <param name="page"></param>
        public static void Append(Page page)
        {
            var newOrderButtonToolBar = new ToolbarItem() { Text = "New Order" };
            
            newOrderButtonToolBar.Clicked += async (sender, e) => {
                await page.Navigation.PushAsync(new NewOrderPage()); ;
            };

            page.ToolbarItems.Add(newOrderButtonToolBar);
        }
    }
}
