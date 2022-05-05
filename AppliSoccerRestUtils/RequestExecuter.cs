using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RestUtils
{
    /// <summary>
    /// This class resposible to make the requests to the server. The server configuration is
    /// passed to the constructor of the class.
    /// </summary>
    public class RequestExecuter
    {
        public ServerConfiguration ServerConfig { get; private set; }
        private RestClient _client;
        public RequestExecuter(ServerConfiguration serverConfiguration)
        {
            ServerConfig = serverConfiguration;
            _client = CreateClient();
        }

        private RestClient CreateClient()
        {
            String address = ServerConfig.Protocol + ServerConfig.HostURL;
            var client = new RestClient(address);
            return client;
        }
        /// <summary>
        /// This method executing the request to the server that was configured.
        /// </summary>
        /// <param name="endPoint">EndPoint object with the name of the endpoint in it, also the 
        /// specific parameters of that endpoint that will be added to the request.</param>
        /// <returns>Task with RestResponse in it.</returns>
        public async Task<RestResponse> ExecuteAsync(EndPoint endPoint)
        {
            var request = new RestRequest(endPoint.GetName());
            AddHeaderParameters(request);
            // Set endpoint parameters
            foreach (var endpointParameter in endPoint.GetEndPointParameters())
            {
                request.AddParameter(endpointParameter.Key, endpointParameter.Value);
            }
            
            RestResponse response = await _client.ExecuteAsync(request);
            return response;
        }

        private void AddHeaderParameters(RestRequest request)
        {
            // Set header info
            foreach (var headerparameter in ServerConfig.HeaderParameters)
            {
                request.AddHeader(headerparameter.Key, headerparameter.Value);
            }
        }

        
    }
}
