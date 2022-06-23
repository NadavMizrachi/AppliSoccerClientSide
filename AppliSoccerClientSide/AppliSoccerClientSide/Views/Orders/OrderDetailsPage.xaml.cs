using AppliSoccerClientSide.Services;
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
    public partial class OrderDetailsPage : ContentPage
    {
        public OrderPayloadViewModel Order { get; set; }
        public OrderDetailsPage(OrderPayload orderToShow)
        {
            InitializeComponent();
            Order = OrderPayloadViewModel.ConvertFromOrderPayload(orderToShow);
            UpdateReceiversListViewHeight();
            BindingContext = this;
        }

        private void UpdateReceiversListViewHeight()
        {
            if (Order == null || Order.Receivers == null)
                return;
            receiversListView.HeightRequest = (Order.Receivers.Count + 2) * receiversListView.RowHeight;
        }

        private void receiversListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (sender is ListView lv) lv.SelectedItem = null;
        }
    }
}