using AppliSoccerClientSide.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using AppliSoccerObjects.Modeling;
using System.Threading.Tasks;
using System.Linq;

namespace AppliSoccerClientSide.Services.Orders
{
    public class ReceiverOrdersPuller : OrdersPuller
    {

        public ReceiverOrdersPuller() : base() { }
        public ReceiverOrdersPuller(int olderBatchSize) : base(olderBatchSize) { }

        protected override Task<List<OrderMetadata>> PullMostRecentOrdersMetadataFromServer(DateTime lowerBoundDate, string askerId)
        {
             return AppliSoccerServerService.AppServer.PullNewOrders(lowerBoundDate, askerId);
        }

        protected override Task<List<OrderMetadata>> PullOldOrdersMetadataFromServer(DateTime upperBoundDate, int quantity, string askerId)
        {
            return AppliSoccerServerService.AppServer.FetchOrdersMetadata(upperBoundDate, quantity, askerId);
        }

    }
}
