using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace RestUtils
{
    public class RestResponseWrraper
    {
        [JsonProperty("response")]
        public JArray Response{ get; set; }
    }
}
