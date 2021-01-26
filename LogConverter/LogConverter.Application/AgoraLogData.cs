using System;
using System.Collections.Generic;
using System.Text;

namespace LogConverter.Application
{
    public class AgoraLogData
    {
        public AgoraLogData(string provider, string httpMethod, int statusCode, string uriPath, string timeTaken, int responseSize, string cacheStatus)
        {
            Provider = provider;
            HttpMethod = httpMethod;
            StatusCode = statusCode;
            UriPath = uriPath;
            TimeTaken = timeTaken.Split(".")[0];
            ResponseSize = responseSize;
            CacheStatus = cacheStatus;
        }

        public string Provider { get; }
        public string HttpMethod { get; }
        public int StatusCode { get; }
        public string UriPath { get; }
        public string TimeTaken { get; }
        public int ResponseSize { get; }
        public string CacheStatus { get; }

        public override string ToString()
        {
            return $"\"{Provider}\" {HttpMethod} {StatusCode} {UriPath} {TimeTaken} {ResponseSize} {CacheStatus}";
        }
    }
}
