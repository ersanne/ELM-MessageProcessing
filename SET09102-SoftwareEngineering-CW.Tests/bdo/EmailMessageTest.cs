using System;
using System.Linq;
using NUnit.Framework;
using SET09102_SoftwareEngineering_CW.bdo;
using SET09102_SoftwareEngineering_CW.exceptions;

namespace SET09102
{
    [TestFixture]
    public class EmailMessageTest
    {
        [Test]
        public void ValidMessageTest()
        {
            var rawMessage = new RawMessage("E123456789", "test@test.com\r\n" +
                                                          "My Subject\r\n" +
                                                          "My Body http://test.com");
            var message = new EmailMessage(rawMessage);

            Assert.AreEqual("E123456789", message.Id);
            Assert.AreEqual("test@test.com", message.Sender);
            Assert.AreEqual("My Subject", message.Subject);
            //Url will only be quarantined if processed using MessageProcessor
            Assert.AreEqual("My Body http://test.com", message.MessageText);
        }

        [Test]
        public void InvalidEmailTest()
        {
            var rawMessage = new RawMessage("E123456789", "test@\r\n" +
                                                          "My Subject\r\n" +
                                                          "My Body http://test.com");
            Assert.Throws<InputException>(delegate { new EmailMessage(rawMessage); });

            rawMessage = new RawMessage("E123456789", "@test\r\n" +
                                                      "My Subject\r\n" +
                                                      "My Body http://test.com");
            Assert.Throws<InputException>(delegate { new EmailMessage(rawMessage); });
        }

        [Test]
        public void NoMessageText()
        {
            var rawMessage = new RawMessage("E123456789", "@test\r\n" +
                                                          "My Subject\r\n");
            Assert.Throws<InputException>(delegate { new EmailMessage(rawMessage); });

            rawMessage = new RawMessage("E123456789", "@test\r\n" +
                                                      "My Subject\r\n" +
                                                      "");
            Assert.Throws<InputException>(delegate { new EmailMessage(rawMessage); });
        }

        [Test]
        [ExpectedException]
        public void MessageTextTooLongTest()
        {
            var rawMessage = new RawMessage("E123456789", "@test\r\n" +
                                                          "My Subject\r\n" +
                                                          RandomString(1100));
            Assert.Throws<InputException>(delegate { new EmailMessage(rawMessage); });
        }
        
        private static string RandomString(int length)
        {
            var random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}