using System;
using System.Collections.Generic;
using System.Text;

namespace RestUtils
{
    /// <summary>
    ///This class holds configurations of the API server. Configurations such as url address
    ///and header parameters. Those configurations should be included on the requests to that server.
    ///In order to use this class, your server should inherint this class and
    ///fill up the configurations with the specific details.
    /// </summary>
    public abstract class ServerConfiguration
    {
        public abstract String Protocol { get; }
        public abstract String HostURL { get; }
        public abstract List<RequestParameter> HeaderParameters { get; }

    }
}
