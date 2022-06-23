using AppliSoccerObjects.Modeling;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AppliSoccerClientSide.Services.Orders
{
    public class SenderOrdersPuller : OrdersPuller
    {
        public SenderOrdersPuller() : base() { }
        public SenderOrdersPuller(int olderBatchSize) : base(olderBatchSize) { }

        protected override Task<List<OrderMetadata>> PullMostRecentOrdersMetadataFromServer(DateTime lowerBoundDate, string askerId)
        {
            return AppliSoccerServerService.AppServer.PullNewSenderOrders(lowerBoundDate, askerId);
        }

        protected override Task<List<OrderMetadata>> PullOldOrdersMetadataFromServer(DateTime upperBoundDate, int quantity, string askerId)
        {
            return AppliSoccerServerService.AppServer.FetchOrdersMetadataForSender(upperBoundDate, quantity, askerId);
        }
    }
}
