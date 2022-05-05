using AppliSoccerConnector;
using System;

namespace Tester
{
    class Program
    {
        static int Main(string[] args)
        {
            IAppServer appServer = new ServerImp();
            var countries = appServer.GetAvailableCountriesAsync().Result;
            return 0;
        }
    }
}
