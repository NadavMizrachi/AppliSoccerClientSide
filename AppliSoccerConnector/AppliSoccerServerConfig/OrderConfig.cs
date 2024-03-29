﻿using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppliSoccerConnector.AppliSoccerServerConfig
{
    public class OrderConfig
    {
        private static readonly string _baseResource = "orders";

        #region CreateOrder
        public static readonly string CreateOrderPath = _baseResource + "/CreateOrder";
        public static readonly Method CreateOrderMethod = Method.PUT;
        // parameter is passing throudh body (JSON) 
        #endregion

        #region FetchOrdersMetadata
        public static readonly string FetchOrdersMetadataPath = _baseResource + "/FetchOrdersMetadata";
        public static readonly Method FetchOrdersMetadataMethod = Method.GET;
        public static readonly string upperBoundDateParamName = "upperBoundDate";
        public static readonly string ordersQuantityParamName = "ordersQuantity";
        public static readonly string receiverIdParamName = "receiverId";
        #endregion

        #region PullNewOrdersMetadata
        public static readonly string PullNewOrdersMetadataPath = _baseResource + "/PullNewOrdersMetadata";
        public static readonly Method PullNewOrdersMetadataMethod = Method.GET;
        public static readonly string lowerBoundDateParamName = "lowerBoundDate";
        #endregion

        #region GetOrderPayload
        public static readonly string GetOrderPayloadPath = _baseResource + "/GetOrderPayload";
        public static readonly Method GetOrderPayloadMethod = Method.GET;
        public static readonly string orderIdParamName = "orderId";
        public static readonly string askerIdParamName = "askerId";
        #endregion

        #region FetchOrdersMetadataForSender
        public static readonly string FetchOrdersMetadataForSenderPath = _baseResource + "/FetchOrdersMetadataForSender";
        public static readonly Method FetchOrdersMetadataForSenderMethod = Method.GET;
        public static readonly string senderIdParamName = "senderId";
        #endregion

        #region PullNewOrdersMetadataForSender
        public static readonly string PullNewOrdersMetadataForSenderPath = _baseResource + "/PullNewOrdersMetadataForSender";
        public static readonly Method PullNewOrdersMetadataForSenderMethod = Method.GET;
        //[HttpGet]
        //public async Task<List<OrderMetadata>> PullNewOrdersMetadataForSender(DateTime lowerBoundDate, int ordersQuantity, String senderId) 
        #endregion

        #region GetOrder
        public static readonly string GetOrderPath = _baseResource + "/GetSentOrder";
        public static readonly Method GetOrderMethod = Method.GET;
        #endregion
    }
}
