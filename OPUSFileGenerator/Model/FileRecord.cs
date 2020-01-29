using System;

namespace OpusFileGenerator.Model
{
    public class FileRecord
    {
        public int MunicipalityNumber { get; set; }
        public int EmploymentType { get; set; }
        public string CprNumber { get; set; }
        public int ExtraNumber { get; set; }
        public string TFCode { get; set; }
        public int DistanceMeters { get; set; }
        public string AccountNumber { get; set; }
        public DateTime DriveDate { get; set; }

        public string GetLine()
        {
            return $"DA6{MunicipalityNumber:0000}{EmploymentType:0}{CprNumber:0000000000}{ExtraNumber:0}{TFCode:0000}{DistanceMeters/10:000000}0000000{AccountNumber,13}{GetEndOfMonth(DriveDate):ddMMyy}";
        }

        private DateTime GetEndOfMonth(DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, DateTime.DaysInMonth(dateTime.Year, dateTime.Month));
        }

    }
}
