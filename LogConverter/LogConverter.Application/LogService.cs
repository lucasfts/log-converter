using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogConverter.Application
{
    public class LogService
    {
        private readonly ILogRepository _logRepository;

        public LogService(ILogRepository logRepository)
        {
            _logRepository = logRepository;
        }

        public async Task<AgoraLog> GetAgoraLogFromMinhaCDN(string minhaCDNSource)
        {
            var version = "1.0";
            var date = new DateTime(2017, 12, 15, 23, 01, 06);
            var provider = "MINHA CDN";

            var minhaCDNLogs = await _logRepository.GetMinhaCDNLogs(minhaCDNSource);

            var agoraLogData = minhaCDNLogs
                .Select(x => new AgoraLogData(provider, x.HttpMethod, x.StatusCode, x.UriPath,
                x.TimeTaken, x.ResponseSize, x.CacheStatus)).ToArray();

            var agoraLog = new AgoraLog(version, date, agoraLogData);

            return agoraLog;
        }

        public void SaveAgoraLogFile(AgoraLog agoraLog, string targetPath)
        {
            CreateFileDirectoryIfNotExists(targetPath);
            File.WriteAllText(targetPath, agoraLog.ToString());
        }

        private static void CreateFileDirectoryIfNotExists(string targetPath)
        {
            var directoryName = Path.GetDirectoryName(targetPath);
            if (!Directory.Exists(directoryName))
            {
                Directory.CreateDirectory(directoryName);
            }
        }
    }
}
