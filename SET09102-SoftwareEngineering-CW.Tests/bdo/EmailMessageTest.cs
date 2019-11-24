using NUnit.Framework;
using SET09102_SoftwareEngineering_CW.bdo;

namespace SET09102
{
    [TestFixture]
    public class EmailMessageTest
    {
        [Test]
        public void ValidMessageTest()
        {
            RawMessage rawMessage = new RawMessage("E123456789", "test@test.com\n" +
                                                                 "My Subject\n" +
                                                                 "My Body http://test.com");
            var message = new EmailMessage(rawMessage);
        }
    }
}