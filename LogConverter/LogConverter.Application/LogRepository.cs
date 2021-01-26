using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace LogConverter.Application
{
    public class LogRepository : ILogRepository
    {
        public async Task<IEnumerable<MinhaCDNLog>> GetMinhaCDNLogs(string minhaCDNSource)
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetStringAsync(minhaCDNSource);
                var responseLines = response.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
                var logs = responseLines.Select(x => new MinhaCDNLog(x));

                return logs;
            }
        }
    }
}