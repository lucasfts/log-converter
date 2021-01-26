using LogConverter.Application;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LogConverter.UnitTests
{
    [TestClass]
    public class LogServiceTests
    {
        Mock<ILogRepository> _logRepository;
        LogService _logService;

        [TestInitialize]
        public void Setup()
        {
            _logRepository = new Mock<ILogRepository>();
            _logService = new LogService(_logRepository.Object);
        }

        [TestMethod]
        public void GetAgoraLogFromMinhaCDN_DataFromValidSource_Success()
        {
            var source = "https://s3.amazonaws.com/uux-itaas-static/minha-cdn-logs/input-01.txt";
            IEnumerable<MinhaCDNLog> minhaCDNLogs = new List<MinhaCDNLog>
            {
                new MinhaCDNLog("312|200|HIT|\"GET /robots.txt HTTP/1.1\"|100.2"),
                new MinhaCDNLog("101|200|MISS|\"POST /myImages HTTP/1.1\"|319.4"),
                new MinhaCDNLog("199|404|MISS|\"GET /not-found HTTP/1.1\"|142.9"),
                new MinhaCDNLog("312|200|INVALIDATE|\"GET /robots.txt HTTP/1.1\"|245.1")
            };
            _logRepository.Setup(x => x.GetMinhaCDNLogs(source)).Returns(Task.FromResult(minhaCDNLogs));

            var expectedLogBuilder = new StringBuilder($"#Version: 1.0{Environment.NewLine}");
            expectedLogBuilder.Append($"#Date: 15/12/2017 23:01:06{Environment.NewLine}");
            expectedLogBuilder.Append($"#Fields: provider http-method status-code uri-path time-taken response-size cache-status {Environment.NewLine}");
            expectedLogBuilder.Append($"\"MINHA CDN\" GET 200 /robots.txt 100 312 HIT{Environment.NewLine}");
            expectedLogBuilder.Append($"\"MINHA CDN\" POST 200 /myImages 319 101 MISS{Environment.NewLine}");
            expectedLogBuilder.Append($"\"MINHA CDN\" GET 404 /not-found 142 199 MISS{Environment.NewLine}");
            expectedLogBuilder.Append($"\"MINHA CDN\" GET 200 /robots.txt 245 312 INVALIDATE");

            var agoraLog = _logService.GetAgoraLogFromMinhaCDN(source).Result;

            Assert.AreEqual(expectedLogBuilder.ToString(), agoraLog.ToString());
        }

        [TestMethod]
        public void GetAgoraLogFromMinhaCDN_DataFromInvalidSource_Error()
        {
            var source = "asdfasdfasdfasdf";
            _logRepository.Setup(x => x.GetMinhaCDNLogs(source)).Throws(new Exception());

            try
            {
                var agoraLog = _logService.GetAgoraLogFromMinhaCDN(source).Result;
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.IsNotNull(ex);
            }
        }

        [TestMethod]
        public void SaveAgoraLogFile_ValidPath_Success()
        {
            var path = "./output/minhaCdn1.txt";

            var log = new AgoraLog("1.0", DateTime.Now, new AgoraLogData[1]);
            _logService.SaveAgoraLogFile(log, path);
        }

        [TestMethod]
        public void SaveAgoraLogFile_InvalidPath_Error()
        {
            var path = "asdfasdfasdfasdf";

            try
            {
                var log = new AgoraLog("1.0", DateTime.Now, new AgoraLogData[1]);
                _logService.SaveAgoraLogFile(log, path);
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.IsNotNull(ex);
            }
        }
    }
}
