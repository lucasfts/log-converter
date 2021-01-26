using System.Collections.Generic;
using System.Threading.Tasks;

namespace LogConverter.Application
{
    public interface ILogRepository
    {
        Task<IEnumerable<MinhaCDNLog>> GetMinhaCDNLogs(string minhaCDNSource);
    }
}