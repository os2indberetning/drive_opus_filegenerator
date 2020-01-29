using System;

namespace OpusFileGenerator.Model
{
    public class Report
    {
        public int Id { get; set; }
        public string CprNumber { get; set; }
        public string TFCode { get; set; }
        public string TFCodeOptional { get; set; }
        public string AccountNumber { get; set; }
        public string EmploymentId { get; set; }
        public int DistanceMeters { get; set; }
        public DateTime DriveDate { get; set; }
        public int ExtraNumber { get; set; }
        public int EmploymentType { get; set; }

        public Report TFCodeOptionalCopy()
        {
            var copy = (Report)this.MemberwiseClone();
            copy.TFCode = TFCodeOptional;
            return copy;
        }
    }
}
