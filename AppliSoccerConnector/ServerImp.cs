using AppliSoccerConnector.AppliSoccerServerConfig;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AppliSoccerConnector
{
    public class ServerImp : IAppServer
    {
        private RestClient _client;
        
        public ServerImp()
        {
            _client = CreateClient();
        }
        private RestClient CreateClient()
        {
            String address = ServerConfig.URL;
            var client = new RestClient(address);
            client.RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
            return client;
        }
        public async Task<IEnumerable<string>> GetAvailableCountriesAsync()
        {
            //var client = new HttpClient();
            //Uri uri = new Uri(ServerConfig.URL + RegistrationConfigs.GetCountriesPath);
            //HttpResponseMessage response = await client.GetAsync(uri);
            //if (response.IsSuccessStatusCode)
            //{
            //    string content = await response.Content.ReadAsStringAsync();

            //}
            //return null;
            var request = new RestRequest(RegistrationConfigs.GetCountriesPath, RegistrationConfigs.GetCountriesMethod);
            var response = await _client.ExecuteAsync<List<string>>(request);
            if (!response.IsSuccessful)
            {
                string errorInfo = "Did not received successfull response for GET_COUNTRIES request. More details: " + response.ErrorMessage +
                    " Inner exception details: " + response.ErrorException.InnerException;
                Console.WriteLine(errorInfo);
                throw new Exception(errorInfo);
            }
            return response.Data;
        }

        public IEnumerable<string> GetAvailableTeamNames(string country)
        {
            throw new NotImplementedException();
        }
    }
}
