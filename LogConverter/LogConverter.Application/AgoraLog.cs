using System;
using System.Collections.Generic;
using System.Text;

namespace LogConverter.Application
{
    public class AgoraLog
    {
        public AgoraLog(string version, DateTime date, AgoraLogData[] logData)
        {
            Version = version;
            Date = date;
            LogData = logData;
        }

        public string Version { get; }
        public DateTime Date { get; }
        public AgoraLogData[] LogData { get; }

        public override string ToString()
        {
            var sb = new StringBuilder($"#Version: {Version}{Environment.NewLine}");
            sb.Append($"#Date: {Date.ToString("dd/MM/yyyy HH:mm:ss")}{Environment.NewLine}");
            sb.Append($"#Fields: provider http-method status-code uri-path time-taken response-size cache-status {Environment.NewLine}");
            
            for (int i = 0; i < LogData.Length; i++)
            {
                sb.Append($"{LogData[i]}");

                if (i < (LogData.Length - 1))
                    sb.Append(Environment.NewLine);
            }

            return sb.ToString();
        }
    }
}
