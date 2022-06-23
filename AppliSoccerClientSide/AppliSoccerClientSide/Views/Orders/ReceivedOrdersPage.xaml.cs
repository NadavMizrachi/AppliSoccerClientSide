using AppliSoccerClientSide.Services;
using AppliSoccerClientSide.Services.Orders;
using AppliSoccerClientSide.ViewModel;
using AppliSoccerClientSide.Views.ViewsUtil;
using AppliSoccerObjects.Modeling;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppliSoccerClientSide.Views.Orders
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ReceivedOrdersPage : ContentPage
    {
        private OrdersPuller _ordersPuller;
        public TeamMember MyMember { get; set; }
        public ObservableCollection<OrderMetadataViewModel> OrdersToDisplay { get; set; }
        public Command LoadMoreOlderOrdersCommand { get;private set; }
        public ICommand RefreshCommand { get; private set; }
        private bool _isRefreshing;
        public bool IsRefreshing
        {
            get => _isRefreshing;
            set
            {
                _isRefreshing = value;
                OnPropertyChanged(nameof(IsRefreshing));
            }
        }
       
        public ReceivedOrdersPage()
        {
            InitializeComponent();
            InitMyMember();
            InitPermissionedElements();
            InitLoadMoreOrdersCommand();
            InitRefreshCommand();
            InitRefreshButtonForWindows();
            InitOrdersPuller();
            BindingContext = this;
        }

        #region Inits
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
        private void InitLoadMoreOrdersCommand()
        {
            LoadMoreOlderOrdersCommand = new Command(LoadOlderOrdersAsync);
        }
        private void InitRefreshCommand()
        {
            RefreshCommand = new Command(PullMostRecentOrders);
        }
        private void InitRefreshButtonForWindows()
        {
            if (Device.RuntimePlatform != Device.UWP)
            {
                return;
            }
            var refreshToolBarItem = new ToolbarItem() { Text = "Refresh" };
            refreshToolBarItem.Clicked += this.RefreshButton_Clicked;
            ToolbarItems.Add(refreshToolBarItem);
        }
        private void InitOrdersPuller()
        {
            _ordersPuller = new ReceiverOrdersPuller();
        }

        #endregion

        #region OnAppearing
        private bool _wasAppeared = false;
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (_wasAppeared)
            {
                return;
            }
            _wasAppeared = true;
            IsBusy = true;

            var fetchedOrdersAsVM = await PullOldOrders();
            OrdersToDisplay = new ObservableCollection<OrderMetadataViewModel>(fetchedOrdersAsVM);
            ordersCollectionView.ItemsSource = OrdersToDisplay;
            IsBusy = false;
        } 
        #endregion

        private async void LoadOlderOrdersAsync()
        {
            if (IsBusy)
                return;
            IsBusy = true;
            var newOrders = await PullOldOrders();
            newOrders.ForEach(order => OrdersToDisplay.Add(order));
            IsBusy = false;
        }

        
        private async Task<List<OrderMetadataViewModel>> PullOldOrders()
        {
            var fetchedOrders = await _ordersPuller.PullNextOldOrdersBatch(MyMember.ID);
            var ordersAsVM = OrderMetadataViewModel.ConvertList(fetchedOrders);
            return ordersAsVM;
        }

        private async void RefreshButton_Clicked(object sender, EventArgs e)
        {
            await Task.Run(() => PullMostRecentOrders());
        }

        private async void PullMostRecentOrders()
        {
            if (IsBusy)
                return;
            List<OrderMetadataViewModel> sortdRecentOrdersMetadataVM = await PullRecentOrders();
            sortdRecentOrdersMetadataVM.ForEach(ovm => OrdersToDisplay.Insert(0, ovm));
            // Stop refreshing
            IsRefreshing = false;
        }

        private async Task<List<OrderMetadataViewModel>> PullRecentOrders()
        {
            var orderMetdata = await _ordersPuller.PullMostRecentOrders(MyMember.ID);
            return OrderMetadataViewModel.ConvertList(orderMetdata);
        }

        private void CollectionView_OnScrolled(object sender, ItemsViewScrolledEventArgs e)
        {
            if (Device.RuntimePlatform != Device.UWP)
                return;

            if (sender is CollectionView cv)
            {
                var count = OrdersToDisplay.Count;
                if (e.LastVisibleItemIndex + 1 - count + cv.RemainingItemsThreshold >= 0)
                {
                    if (cv.RemainingItemsThresholdReachedCommand.CanExecute(null))
                        cv.RemainingItemsThresholdReachedCommand.Execute(null);
                }
            }
        }

        private async void ordersCollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            OrderMetadataViewModel selectedOrder = e.CurrentSelection.FirstOrDefault() as OrderMetadataViewModel;
            if(selectedOrder == null)
                return;
            OrderPayload orderPayload;
            orderPayload = await AppliSoccerServerService.AppServer.GetOrderPayload(selectedOrder.Id, MyMember.ID);
            if(orderPayload == null)
            {
                await DisplayAlert("Error!", "Cannot open order details!", "cancel");
            }
            else
            {
                await Navigation.PushAsync(new OrderDetailsPage(orderPayload));
                selectedOrder.WasRead = true;
            }
            // Clean
            ((CollectionView)sender).SelectedItem = null;
        }

    }
}