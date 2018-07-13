using GeoNerves.Models;
using NUnit.Framework;

namespace GeoNerves.Tests
{
    [TestFixture()]
    public class AddressApiResponseTests
    {
        [Test]
        public void AddressApiResponse_VerifyCsvFactory()
        {
            var addressResponseString = "\"3\",\"216 Norfolk Street, Cambridge, MA, 02139\",\"Match\",\"Exact\",\"216 NORFOLK ST, CAMBRIDGE, MA, 02139\",\"-71.098366,42.369587\",\"86869427\",\"R\"";
            var addressResponse = AddressApiResponse.ParseAddressApiResponseFromCsv(addressResponseString);

            Assert.IsTrue(addressResponse.Address.City == "Cambridge");
        }
    }
}
