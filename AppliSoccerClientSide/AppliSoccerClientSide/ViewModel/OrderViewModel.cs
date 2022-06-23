using AppliSoccerObjects.Modeling;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppliSoccerClientSide.ViewModel
{
    public class OrderViewModel
    {
        public DateTime SendingDate { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public List<string> ReceiversNames { get; set; }
        public List<ReceiverInfo> ReceiverInfos { get; set; }

        public static OrderViewModel Create(SentOrderWithReceiversInfo sentOrderWithReceiversInfo, List<string> receiversNames)
        {
            return new OrderViewModel
            {
                Content = sentOrderWithReceiversInfo.Order.Content,
                Title = sentOrderWithReceiversInfo.Order.Title,
                SendingDate = sentOrderWithReceiversInfo.Order.SendingDate,
                ReceiversNames = receiversNames,
                ReceiverInfos = sentOrderWithReceiversInfo.ReceiverInfos
            };
        }
    }
}
