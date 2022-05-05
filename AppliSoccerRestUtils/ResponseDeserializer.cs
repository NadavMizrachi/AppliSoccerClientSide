using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace RestUtils
{
    public class ResponseDeserializer
    {

        // The response has 'response' field inside it.
        // In this field, there is an array of objects, therefore we need 
        // return a list of objects.
        public static List<T> DeserializeAsObjectList<T>(RestResponse response)
        {
            if (!response.IsSuccessful)
            {
                throw new Exception("Unsuccessfull request!");
            }
            RestResponseWrraper wrraper = 
                JsonConvert.DeserializeObject<RestResponseWrraper>(response.Content);
            List<T> deserialized = JsonConvert.DeserializeObject<List<T>>(wrraper.Response.ToString());
            return deserialized;
        }
    }
}
