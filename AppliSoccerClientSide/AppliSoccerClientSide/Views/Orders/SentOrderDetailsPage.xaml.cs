using AppliSoccerClientSide.ViewModel;
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
    public partial class SentOrderDetailsPage : ContentPage
    {
        public OrderViewModel Order { get; set; }
        public SentOrderDetailsPage(SentOrderWithReceiversInfo order, List<string> receiversNames)
        {
            InitializeComponent();
            Order = OrderViewModel.Create(order, receiversNames);
            UpdateReceiverInfosListViewHeight();
            BindingContext = this;
        }
        private void UpdateReceiverInfosListViewHeight()
        {
            if (Order == null || Order.ReceiverInfos == null)
                return;
            receiverInfosListView.HeightRequest = (Order.ReceiverInfos.Count + 2) * receiverInfosListView.RowHeight;
        }
    }
}