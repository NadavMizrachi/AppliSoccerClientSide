using AppliSoccerObjects.Modeling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppliSoccerClientSide.Services.Orders
{
    /// <summary>
    /// Abstract class that represents pulling algorithm template. 
    /// The class provides orders/orderMetadata. The class provides
    /// Batches (with fixed size) of older orders, and
    /// list of new orders (no restricted to fixed size) that created while the application runs.
    /// </summary>
    public abstract class OrdersPuller
    {
        private const int DEF_OLD_OREDRS_BATCH_SIZE = 10;

        private int _oldOrdersBatchSize = DEF_OLD_OREDRS_BATCH_SIZE;
        private DateTime _earliestOrderDate = DateTime.MaxValue;
        private DateTime _latestOrderDate = DateTime.MinValue;
        public bool NoMoreOlderOrders
        {
            get;
            private set;
        }
        public OrdersPuller() { }
        public OrdersPuller(int oldOrdersBatchSize)
        {
            _oldOrdersBatchSize = oldOrdersBatchSize;
        }


        protected abstract Task<List<OrderMetadata>> PullOldOrdersMetadataFromServer(DateTime upperBoundDate, int quantity, string askerId);
        protected abstract Task<List<OrderMetadata>> PullMostRecentOrdersMetadataFromServer(DateTime lowerBoundDate, string askerId);
        
        /// <summary>
        /// Pulls next batch of older orders, returns the list of the orders sorted descending order by
        /// date
        /// </summary>
        /// <param name="memberId"></param>
        /// <returns></returns>
        public async Task<List<OrderMetadata>> PullNextOldOrdersBatch(string memberId)
        {
            if (NoMoreOlderOrders)
                return new List<OrderMetadata>();

            // Send to server the upper bound of the earliest order date we have + number of orders to fetch
            List<OrderMetadata> nextOrderMetadataList =
                await PullOldOrdersMetadataFromServer(_earliestOrderDate, _oldOrdersBatchSize, memberId);
            // Convert Server metadata to view model
            if (nextOrderMetadataList == null || nextOrderMetadataList.Count == 0)
            {
                NoMoreOlderOrders = true;
                return new List<OrderMetadata>();
            }
            // Sort the orders we got in descanding order (by date)
            nextOrderMetadataList.OrderByDescending(o => o.SentDate).ToList();
            // Save the earliest order date
            DateTime earliestOfCurrentBatch = nextOrderMetadataList.Last().SentDate;
            DateTime latestOfCurrentBatch = nextOrderMetadataList.First().SentDate;
            // Update early/late date in order to track the pulling

            PrintDates(nextOrderMetadataList);

            _earliestOrderDate = _earliestOrderDate < earliestOfCurrentBatch ? _earliestOrderDate : earliestOfCurrentBatch;
            _latestOrderDate = _latestOrderDate > latestOfCurrentBatch ? _latestOrderDate : latestOfCurrentBatch;

            //return output;
            return nextOrderMetadataList;
        }

        private void PrintDates(List<OrderMetadata> nextOrderMetadataList)
        {
            nextOrderMetadataList.ForEach(o => Console.WriteLine($"Time: {o.SentDate} Kind: {o.SentDate.Kind}"));
        }

        /// <summary>
        /// Pulls all most recent orders. Returns list of the new orders order by date (from earliest to oldest).
        /// </summary>
        /// <param name="memberId"></param>
        /// <returns></returns>
        public async Task<List<OrderMetadata>> PullMostRecentOrders(string memberId)
        {
            List<OrderMetadata> newOrders = await PullMostRecentOrdersMetadataFromServer(_latestOrderDate, memberId);
            if (newOrders == null || newOrders.Count == 0)
            {
                return new List<OrderMetadata>();
            }
            newOrders.OrderBy(o => o.SentDate);
            DateTime latestOfCurrentBatch = newOrders.Last().SentDate;
            _latestOrderDate = _latestOrderDate > latestOfCurrentBatch ? _latestOrderDate : latestOfCurrentBatch;

            return newOrders;
        }

       
    }
}
