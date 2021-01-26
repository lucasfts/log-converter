using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LogConverter.Application
{
    public class MinhaCDNLog
    {
        public MinhaCDNLog(string logText)
        {
            var columns = logText.Split("|").Select(x => x.Trim()).ToArray();

            ResponseSize = int.Parse(columns[0]);
            StatusCode = int.Parse(columns[1]);
            CacheStatus = columns[2];

            var requestColumns = columns[3].Replace("\"", string.Empty).Split(" ");
            HttpMethod = requestColumns[0];
            UriPath = requestColumns[1];

            TimeTaken = columns[4];
        }

        public int ResponseSize { get; }
        public int StatusCode { get; }
        public string CacheStatus { get; }
        public string HttpMethod { get; }
        public string UriPath { get; }
        public string TimeTaken { get; }

        public override string ToString()
        {
            return $"{ResponseSize} | ${StatusCode} | {CacheStatus} | \"{HttpMethod} {UriPath}\" {TimeTaken}";
        }
    }
}
