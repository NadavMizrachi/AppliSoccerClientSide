using AppliSoccerClientSide.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using AppliSoccerObjects.Modeling;
using System.Threading.Tasks;
using System.Linq;

namespace AppliSoccerClientSide.Services.Orders
{
    public class OrdersPuller
    {
        private const int DEF_OLD_OREDRS_BATCH_SIZE = 20;

        private int _oldOrdersBatchSize = DEF_OLD_OREDRS_BATCH_SIZE;
        private DateTime _earliestOrderDate = DateTime.MaxValue;
        private DateTime _latestOrderDate = DateTime.MinValue;

        public bool NoMoreOlderOrders
        {
            get;
            private set;
        }
        public OrdersPuller()
        {

        }

        public OrdersPuller(int oldOrdersBatchSize)
        {
            _oldOrdersBatchSize = oldOrdersBatchSize;
        }

        /// <summary>
        /// Pulls next batch of older orders, returns the list of the orders sorted descending order by
        /// date
        /// </summary>
        /// <param name="memberId"></param>
        /// <returns></returns>
        public async Task<List<OrderMetadataViewModel>> PullNextOldOrdersBatch(string memberId)
        {
            // Send to server the upper bound of the earliest order date we have + number of orders to fetch
            List<OrderMetadata> nextOrderMetadataList =
                await AppliSoccerServerService
                .AppServer
                .FetchOrdersMetadata(_earliestOrderDate, _oldOrdersBatchSize, memberId);
            // Convert Server metadata to view model
            if (nextOrderMetadataList == null || nextOrderMetadataList.Count == 0)
            {
                NoMoreOlderOrders = true;
                // return empty list
                return new List<OrderMetadataViewModel>();
            }
            List<OrderMetadataViewModel> nextOrders = OrderMetadataViewModel.ConvertList(nextOrderMetadataList);
            // Sort the orders we got in descanding order (by date)
            nextOrders.OrderByDescending(o => o.SentDate).ToList();
            // Save the earliest order date
            DateTime earliestOfCurrentBatch = nextOrders.Last().SentDate;
            DateTime latestOfCurrentBatch = nextOrders.First().SentDate;

            // Update early/late date in order to track the pulling
            _earliestOrderDate = _earliestOrderDate < earliestOfCurrentBatch ? _earliestOrderDate : earliestOfCurrentBatch;
            _latestOrderDate = _latestOrderDate > latestOfCurrentBatch ? _latestOrderDate : latestOfCurrentBatch;

            //return output;
            return nextOrders;
        }

        /// <summary>
        /// Pulls all most recent orders. Returns list of the new orders order by date (from earliest to oldest).
        /// </summary>
        /// <param name="memberId"></param>
        /// <returns></returns>
        public async Task<List<OrderMetadataViewModel>> PullMostRecentOrders(string memberId)
        {
            List<OrderMetadata> newOrders =
                await AppliSoccerServerService
                .AppServer
                .PullNewOrders(_latestOrderDate, memberId);
            List<OrderMetadataViewModel> newOrdersMetadataVM = OrderMetadataViewModel.ConvertList(newOrders);
            newOrdersMetadataVM.OrderBy(ovm => ovm.SentDate);
            DateTime latestOfCurrentBatch = newOrdersMetadataVM.Last().SentDate;
            _latestOrderDate = _latestOrderDate > latestOfCurrentBatch ? _latestOrderDate : latestOfCurrentBatch;

            return newOrdersMetadataVM;
        }
    }
}
