using Microsoft.Extensions.DependencyInjection;
using System;

namespace LogConverter.Application
{
    class Program
    {
        private static LogService _logService;

        static void Main(string[] args)
        {
            RegisterDependencies();

            string commandText = "";
            Console.WriteLine("Type \"exit\" to finish the application");
            while (commandText.ToLower() != "exit")
            {
                commandText = Console.ReadLine();
                var commandParams = commandText.Split(" ", StringSplitOptions.RemoveEmptyEntries);

                if (commandParams.Length == 3 && commandParams[0].ToLower() == "convert")
                {
                    // convert https://s3.amazonaws.com/uux-itaas-static/minha-cdn-logs/input-01.txt ./output/minhaCdn1.txt
                    Convert(commandParams[1], commandParams[2]);
                }
            }

        }

        private static void RegisterDependencies()
        {
            var serviceProvider = new ServiceCollection()
                       .AddSingleton<LogService>()
                       .AddSingleton<ILogRepository, LogRepository>()
                       .BuildServiceProvider();


            _logService = serviceProvider.GetService<LogService>();
        }

        private static void Convert(string sourceUrl, string targetPath)
        {
            var agoraLog = _logService.GetAgoraLogFromMinhaCDN(sourceUrl).Result;
            _logService.SaveAgoraLogFile(agoraLog, targetPath);
        }
    }
}
