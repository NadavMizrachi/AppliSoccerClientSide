using AppliSoccerClientSide.Helpers.Mocking;
using AppliSoccerClientSide.Services;
using AppliSoccerClientSide.ViewModel;
using AppliSoccerClientSide.Views.ViewsUtil;
using AppliSoccerObjects.Modeling;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppliSoccerClientSide.Views.Orders
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ReceivedOrdersPage : ContentPage
    {
        private int _batchSize = 20;
        private bool _noMoreOrders = false;
        private DateTime _earliestOrderDate = DateTime.MaxValue;
        private DateTime _latestOrderDate = DateTime.MinValue;

        public TeamMember MyMember { get; set; }
        public ObservableCollection<OrderMetadataViewModel> OrdersToDisplay { get; set; }
        public Command LoadMoreOrdersCommand { get;private set; }
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
            LoadMoreOrdersCommand = new Command(LoadOlderOrdersAsync);
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

        #endregion

        private async void LoadOlderOrdersAsync()
        {
            if (IsBusy || _noMoreOrders)
            {
                return;
            }
            IsBusy = true;

            var newOrders = await Task.Run(() => GetNextOrders());
            if (newOrders == null || newOrders.Count == 0)
            {
                _noMoreOrders = true;

            }
            else
            {
                newOrders.ForEach(order => OrdersToDisplay.Add(order));
            }
            IsBusy = false;
        }

        private bool _wasAppeared = false;
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (_wasAppeared)
            {
                return;
            }
            _wasAppeared = true;
            var fetchedOrders = await Task.Run(() => GetNextOrders());
            OrdersToDisplay = new ObservableCollection<OrderMetadataViewModel>(fetchedOrders);
            if (fetchedOrders != null && fetchedOrders.Count > 0)
            {
                _latestOrderDate = fetchedOrders.First().SentDate;
            }
            ordersCollectionView.ItemsSource = OrdersToDisplay;
        }

        private List<OrderMetadataViewModel> GetNextOrders()
        {
            // Send to server the upper bound of the earliest order date we have + number of orders to fetch
            List<OrderMetadata> nextOrderMetadataList =
                AppliSoccerServerService
                .AppServer
                .FetchOrdersMetadata(_earliestOrderDate, _batchSize, receiverId: MyMember.ID)
                .Result;
            // Convert Server metadata to view model
            if (nextOrderMetadataList == null || nextOrderMetadataList.Count == 0)
            {
                // return empty list
                return new List<OrderMetadataViewModel>();
            }
            List<OrderMetadataViewModel> nextOrders = ConvertOrderMetadataToViewModel(nextOrderMetadataList);
            // Sort the orders we got in descanding order (by date)
            nextOrders.OrderByDescending(o => o.SentDate).ToList();
            // Save the earliest order date
            _earliestOrderDate = nextOrders.Last().SentDate;

            //return output;
            return nextOrders;
        }

        private List<OrderMetadataViewModel> ConvertOrderMetadataToViewModel(List<OrderMetadata> orderMetadatas)
        {
            return orderMetadatas
                .ConvertAll(omd => OrderMetadataViewModel.Convert(omd))
                .ToList();
        }
        private async void RefreshButton_Clicked(object sender, EventArgs e)
        {
            await Task.Run(() => PullMostRecentOrders());
        }

        private async void PullMostRecentOrders()
        {
            if (IsBusy)
            {
                return;
            }

            // Pull ....
            List<OrderMetadata> newOrders = await AppliSoccerServerService.AppServer.PullNewOrders(_latestOrderDate, MyMember.ID);
            if (newOrders != null && newOrders.Count > 0)
            {
                List<OrderMetadataViewModel> newOrdersMetadataVM = ConvertOrderMetadataToViewModel(newOrders);
                newOrdersMetadataVM.OrderBy(ovm => ovm.SentDate);
                _latestOrderDate = newOrdersMetadataVM.Last().SentDate;
                newOrdersMetadataVM.ForEach(ovm => OrdersToDisplay.Insert(0, ovm));
            }

            // Stop refreshing
            IsRefreshing = false;
        }


        private void CollectionView_OnScrolled(object sender, ItemsViewScrolledEventArgs e)
        {
            if (Device.RuntimePlatform != Device.UWP)
            {
                return;
            }

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
            {
                return;
            }
            OrderPayload orderPayload;
            orderPayload = await Task.Run(() => {
                return GetOrderFromServer(selectedOrder.Id);
            });
            if(orderPayload == null)
            {
                await DisplayAlert("Error!", "Cannot open order details!", "cancel");
            }
            else
            {
                await Navigation.PushAsync(new OrderDetailsPage(orderPayload));
                selectedOrder.WasRead = true;
            }

            ((CollectionView)sender).SelectedItem = null;
        }

        private OrderPayload GetOrderFromServer(string orderId)
        {
            OrderPayload order = AppliSoccerServerService.AppServer.GetOrderPayload(orderId, MyMember.ID).Result;
            return order;
        }
    }
}