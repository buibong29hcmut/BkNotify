using BkMail.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BkMail.Test
{
    [TestClass]   
    public class TestConvertUnixTimeToGMT7Time
    {
        [TestMethod]
        public void Convert()
        {
           var result= ConvertDateTimeUnixToNormalDatetime.UnixTimeStampToDateTime(1662724446);
            Console.WriteLine(result);
            Assert.AreEqual("a", "a");
        }
    }
}
