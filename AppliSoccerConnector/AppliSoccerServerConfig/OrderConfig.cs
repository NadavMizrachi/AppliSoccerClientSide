using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppliSoccerConnector.AppliSoccerServerConfig
{
    public class OrderConfig
    {
        private static readonly string _baseResource = "orders";

        public static readonly string CreateOrderPath = _baseResource + "/CreateOrder";
        public static readonly Method CreateOrderMethod = Method.PUT;
        // parameter is passing throudh body (JSON)
    }
}
