using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpusFileGenerator.Model;

namespace OpusFileGeneratorTest
{
    [TestClass]
    public class FileRecordTest
    {
        [TestMethod]
        [DataRow("9999999999")]
        [DataRow(null)]
        public void GetLineReturnsExpectedString(string accountNumber)
        {
            var record = new FileRecord()
            {
                MunicipalityNumber = 123,
                EmploymentType = 9,
                CprNumber = "8512481230",
                ExtraNumber = 5,
                TFCode = "6666",
                DistanceMeters = 123500,
                DriveDate = new DateTime(2020, 1, 27),
                AccountNumber = accountNumber
            };
            var expected = accountNumber == null ? "DA6012398512481230566660123500000000             310120" : "DA6012398512481230566660123500000000   9999999999310120";
            var actual = record.GetLine();
            Assert.AreEqual(expected, actual);
        }
    }
}
