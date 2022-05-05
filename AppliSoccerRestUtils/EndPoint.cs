using System;
using System.Collections.Generic;

namespace RestUtils
{
    /// <summary>
    ///This class represnt endpoint (resource) in the API server.
    /// </summary>
    public abstract class EndPoint
    {
        protected string _name;
        private List<RequestParameter> _parameters;

        public EndPoint()
        {
            _parameters = new List<RequestParameter>();
        }

        public void SetParameter(String key, String value)
        {
            if(_parameters == null)
            {
                _parameters = new List<RequestParameter>();
            }

            _parameters.Add(
                    new RequestParameter() { Key = key, Value = value }
                );
        }

        public List<RequestParameter> GetEndPointParameters()
        {
            return _parameters;
        }

        public string GetName()
        {
            return _name;
        }

    }
}