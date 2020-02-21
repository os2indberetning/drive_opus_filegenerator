using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using OpusFileGenerator.Model;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace OpusFileGenerator.Service
{
    public class OS2indberetningService
    {
        private readonly ILogger logger;
        private readonly IConfiguration configuration;


        public OS2indberetningService(ILogger<OS2indberetningService> logger, IConfiguration configuration)
        {
            this.logger = logger;
            this.configuration = configuration;
        }

        public IEnumerable<Report> GetReports()
        {
            using (var webClient = GetWebClient())
            {
                var reportData = webClient.DownloadString(configuration["ApiBaseUrl"] + "GetReportsToPayroll");
                var reports = JsonConvert.DeserializeObject<IEnumerable<Report>>(reportData);
                logger.LogInformation($"Fetched {reports.Count()} reports");
                return reports;
            }
        }

        public void AcknowledgeReports(IEnumerable<Report> reports)
        {
            using (var webClient = GetWebClient())
            {

                var json = JsonConvert.SerializeObject(reports.Select(r => r.Id));
                webClient.UploadString(configuration["ApiBaseUrl"] + "AcknowledgeReportsProcessed", json);
                logger.LogInformation($"Acknowledged {reports.Count()} reports");
            }
        }

        private WebClient GetWebClient()
        {
            var webClient = new WebClient();
            webClient.Headers[HttpRequestHeader.ContentType] = "application/json";
            webClient.Headers["ApiKey"] = configuration["ApiKey"];
            webClient.Encoding = Encoding.UTF8;
            return webClient;
        }
    }
}
