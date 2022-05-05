using AppliSoccerConnector;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppliSoccerClientSide.Services
{
    public class AppliSoccerServerService
    {
        private static IAppServer _appServer;

        public static IAppServer AppServer
        {
            get
            {
                if(_appServer == null)
                {
                    _appServer = new ServerImp();
                }
                return _appServer;
            }
        }
    }
}
