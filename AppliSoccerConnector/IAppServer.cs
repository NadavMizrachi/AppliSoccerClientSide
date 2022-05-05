using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AppliSoccerConnector
{
    public interface IAppServer
    {
        Task<IEnumerable<string>> GetAvailableCountriesAsync();
        IEnumerable<string> GetAvailableTeamNames(string country);
    }
}
