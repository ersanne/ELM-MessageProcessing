using System;
using ELMPrototype.Tests.bdo;
using ELMPrototype.Tests.exceptions;
using NUnit.Framework;
using Newtonsoft.Json;

namespace ELMPrototype.Tests.service
{
    public class MessageProcessorTest
    {
        private MessageProcessor _messageProcessor;

        public MessageProcessorTest()
        {
            _messageProcessor = MessageProcessor.GetInstance();
        }

        [Test]
        public void InvalidMessageTypeTest()
        {
            var rawMessage = new RawMessage("Z123456789", "Test");

            var ex = Assert.Throws<InputException>(delegate
            {
                _messageProcessor.ProcessMessageIndentedJson(rawMessage);
            });
            Assert.AreEqual(ex.Message,
                "Could not detect message type. Please ensure the message is correctly formatted.");
        }

        [Test]
        public void ProcessSmsMessageTest()
        {
            var rawMessage = new RawMessage("S123456789", "+441234567891" + Environment.NewLine +
                                                          "This is a text LOL");

            var json = _messageProcessor.ProcessMessageIndentedJson(rawMessage);

            var messsage = JsonConvert.DeserializeObject<SMSMessage>(json);
            Assert.AreEqual("S123456789", messsage.Id);
            Assert.AreEqual("+441234567891", messsage.Sender);
            Assert.AreEqual("This is a text LOL <Laughing out loud>", messsage.MessageText);
        }

        [Test]
        public void ProcessStandardEmailMessageTest()
        {
            var rawMessage = new RawMessage("E123456789", "test@test.com" + Environment.NewLine +
                                                          "Test subject" + Environment.NewLine +
                                                          "This is a test http://www.eriksanne.com https://www.eriksanne.com");

            var json = _messageProcessor.ProcessMessageIndentedJson(rawMessage);

            var messsage = JsonConvert.DeserializeObject<EmailMessage>(json);
            Assert.AreEqual("E123456789", messsage.Id);
            Assert.AreEqual("test@test.com", messsage.Sender);
            Assert.AreEqual("Test subject", messsage.Subject);
            Assert.AreEqual("This is a test <URL Quarantined> <URL Quarantined>", messsage.MessageText);
        }

        [Test]
        public void ProcessSirEmailMessageTest()
        {
            var rawMessage = new RawMessage("E123456789", "test@test.com" + Environment.NewLine +
                                                          "SIR 24/11/2019" + Environment.NewLine +
                                                          "66-666-66" + Environment.NewLine +
                                                          "Theft of Properties" + Environment.NewLine +
                                                          "This is a test http://www.eriksanne.com https://www.eriksanne.com");

            var json = _messageProcessor.ProcessMessageIndentedJson(rawMessage);

            var messsage = JsonConvert.DeserializeObject<EmailMessage>(json);
            Assert.AreEqual("E123456789", messsage.Id);
            Assert.AreEqual("test@test.com", messsage.Sender);
            Assert.AreEqual("SIR 24/11/2019", messsage.Subject);
            Assert.AreEqual("66-666-66", messsage.SportCentreCode);
            Assert.AreEqual("Theft of Properties", messsage.IncidentType);
            Assert.AreEqual("This is a test <URL Quarantined> <URL Quarantined>", messsage.MessageText);
        }

        [Test]
        public void ProcessTweetMessageTest()
        {
            var rawMessage = new RawMessage("T123456789", "@Test" + Environment.NewLine +
                                                          "This is a #test @Test #test @Test");

            var json = _messageProcessor.ProcessMessageIndentedJson(rawMessage);

            var messsage = JsonConvert.DeserializeObject<TweetMessage>(json);
            Assert.AreEqual("T123456789", messsage.Id);
            Assert.AreEqual("@Test", messsage.Sender);
            Assert.AreEqual("This is a #test @Test #test @Test", messsage.MessageText);
        }
    }
}