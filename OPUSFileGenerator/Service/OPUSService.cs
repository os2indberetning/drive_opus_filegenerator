using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OpusFileGenerator.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace OpusFileGenerator.Service
{
    public class OPUSService
    {
        private IConfiguration configuration;
        private ILogger logger;
        
        public OPUSService(IConfiguration configuration, ILogger<OPUSService> logger)
        {
            this.configuration = configuration;
            this.logger = logger;
        }
        public void GenerateReportFile(IEnumerable<Report> reports)
        {
            var records = GenerateFileRecords(reports);
            WriteRecordsToFile(records);
        }

        private IEnumerable<FileRecord> GenerateFileRecords(IEnumerable<Report> reports)
        {
            var reportList = reports.ToList();
            // If a Drive report has an TFCodeOptional, we want to make two records. 
            // One using the real TFCode and one using the TFCodeOptional
            reportList.AddRange(reports.Where(r => r.TFCodeOptional != null).Select(r => r.TFCodeOptionalCopy()));

            return reportList.GroupBy(r => new { r.EmploymentId, r.TFCode, r.AccountNumber, r.DriveDate.Year, r.DriveDate.Month }, (key, group) => new FileRecord
            {
                MunicipalityNumber = configuration.GetValue<int>("MunicipalityNumber"),
                TFCode = key.TFCode,
                AccountNumber = key.AccountNumber,
                DistanceMeters = group.Sum(r => r.DistanceMeters),
                EmploymentType = group.First().EmploymentType,
                CprNumber = group.First().CprNumber,
                ExtraNumber = group.First().ExtraNumber,
                DriveDate = group.First().DriveDate,
            });

        }

        private void WriteRecordsToFile(IEnumerable<FileRecord> records)
        {
            var outFilePath = Path.Combine(configuration["FilePath"], configuration["FileName"]);
            var lines = new List<string>();
            if (!File.Exists(outFilePath))
            {
                lines.Add(configuration["FileHeader"]);
            }
            lines.AddRange(records.Select(x => x.GetLine()));
            File.AppendAllLines(outFilePath, lines);
            logger.LogInformation($"Appended {records.Count()} lines to {outFilePath}");

            var backupFilePath = Path.Combine(configuration["FilePath"], "Backup", $"{DateTime.Now:yyyyMMddTHHmmss}_{configuration["FileName"]}");
            Directory.CreateDirectory(Path.GetDirectoryName(backupFilePath));
            File.Copy(outFilePath, backupFilePath);
            logger.LogInformation($"Created backup file {backupFilePath}");
        }
    }
}
