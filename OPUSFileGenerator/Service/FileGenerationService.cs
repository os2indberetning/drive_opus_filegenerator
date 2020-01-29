using Microsoft.Extensions.Logging;
using Topshelf;

namespace OpusFileGenerator.Service
{
    public class FileGenerationService : ServiceControl
    {
        private readonly ILogger<FileGenerationService> _logger;
        public FileGenerationService(ILogger<FileGenerationService> logger)
        {
            _logger = logger;
        }
        public bool Start(HostControl hostControl)
        {
            _logger.LogInformation("Service started");
            return true;
        }

        public bool Stop(HostControl hostControl)
        {
            _logger.LogInformation("Service stopped");
            return true;
        }
    }
}
