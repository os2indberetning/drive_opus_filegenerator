using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpusFileGenerator.Utility;
using Quartz;
using System;
using System.Linq;

namespace OpusFileGenerator.Service
{
    [DisallowConcurrentExecution]
    public class FileGenerationJob : IJob
    {
        private readonly ILogger logger;
        private readonly OS2indberetningService os2indberetningService;
        private readonly OPUSService opusService;
        private bool acknowledgeSucceeded = true;
        public FileGenerationJob(IServiceProvider provider)
        {
            logger = provider.GetService<ILogger<FileGenerationJob>>();
            os2indberetningService = provider.GetService<OS2indberetningService>();
            opusService = provider.GetService<OPUSService>();
        }
        public void Execute(IJobExecutionContext context)
        {
            try
            {
                if (!acknowledgeSucceeded)
                {
                    throw new Exception("Previous job execution was not acknowledged. Job halted.");
                }
                var reports = os2indberetningService.GetReports();
                if (reports.Count() > 0)
                {
                    acknowledgeSucceeded = false;
                    opusService.GenerateReportFile(reports);
                    Retry.Do(() => os2indberetningService.AcknowledgeReports(reports), TimeSpan.FromSeconds(10));
                    acknowledgeSucceeded = true;
                }
            }
            catch (Exception e)
            {
                logger.LogError(e, "Job execution failed");
            }
        }
    }
}
