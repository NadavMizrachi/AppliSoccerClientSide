using AppliSoccerClientSide.Services;
using AppliSoccerClientSide.Views.ViewsUtil;
using AppliSoccerObjects.Modeling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppliSoccerClientSide.Views.Orders
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ReceivedOrdersPage : ContentPage
    {
        public TeamMember MyMember { get; set; }

        // TODO - Coach could not get orders, so this page should not be visible to coach user.
        public ReceivedOrdersPage()
        {
            InitializeComponent();
            InitMyMember();
            InitPermissionedElements();
        }

        private void InitMyMember()
        {
            MyMember = ApplicationGlobalData.GetMyTeamMember();
        }

        private void InitPermissionedElements()
        {
            ViewsPermissionManager viewsPermissionManager =
                ViewsPermissionManager.CreateManager();
            if (viewsPermissionManager.IsPermissionedForNewOrderButton)
            {
                NewOrderButtonBarAppender.Append(page: this);
            }
        }
    }
}