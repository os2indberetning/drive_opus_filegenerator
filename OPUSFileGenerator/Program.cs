using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Quartz;
using OpusFileGenerator.Service;
using Topshelf;
using Topshelf.MicrosoftDependencyInjection;
using Topshelf.Quartz;
using Serilog;

namespace OpusFileGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            Log.Logger = new LoggerConfiguration().WriteTo.File(configuration["LogFile"]).CreateLogger();

            IServiceCollection services = new ServiceCollection();
            services.AddSingleton<IConfiguration>(configuration);
            services.AddSingleton<FileGenerationService>();
            services.AddSingleton<FileGenerationJob>();
            services.AddSingleton<OS2indberetningService>();
            services.AddSingleton<OPUSService>();
            services.AddLogging(logging => {
                logging.ClearProviders();
                logging.AddSerilog();
                logging.AddDebug();
                logging.SetMinimumLevel(LogLevel.Debug);
            });
            var serviceProvider = services.BuildServiceProvider();

            HostFactory.Run(h =>
            {
                h.UseMicrosoftDependencyInjection(serviceProvider);
                h.UsingQuartzJobFactory(() => new MicrosoftDependencyInjectionJobFactory(serviceProvider));
                h.Service<FileGenerationService>(sc =>
                {
                    sc.ConstructUsingMicrosoftDependencyInjection();
                    sc.WhenStarted((s, hostControl) => s.Start(hostControl));
                    sc.WhenStopped((s, hostControl) => s.Stop(hostControl));
                    sc.ScheduleQuartzJob(q => q.WithJob(() => JobBuilder.Create<FileGenerationJob>().Build())
                        .AddTrigger(() => TriggerBuilder.Create()
                        .WithCronSchedule(configuration["CronSchedule"])
                        .Build())
                        .AddTrigger(() => TriggerBuilder.Create().StartNow().Build()));
                });
                h.SetDisplayName("OS2indberetning OPUS file generator");
                h.SetServiceName("OS2indberetning OPUS file generator");
                h.SetDescription("Generates OPUS file with drive report data");
            });
        }
    }
}
