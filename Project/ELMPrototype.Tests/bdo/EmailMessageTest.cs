﻿using System;
using System.Linq;
using ELMPrototype.bdo;
using ELMPrototype.exceptions;
using NUnit.Framework;

namespace ELMPrototype.Tests
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
            var rawMessage = new RawMessage("E123456789", "test@" + Environment.NewLine +
                                                          "My Subject" + Environment.NewLine +
                                                          "My Body http://test.com");
            Assert.Throws<InputException>(delegate { new EmailMessage(rawMessage); });

            rawMessage = new RawMessage("E123456789", "@test" + Environment.NewLine +
                                                      "My Subject" + Environment.NewLine +
                                                      "My Body http://test.com");
            Assert.Throws<InputException>(delegate { new EmailMessage(rawMessage); });
        }

        [Test]
        public void NoMessageText()
        {
            var rawMessage = new RawMessage("E123456789", "test@test.com" + Environment.NewLine +
                                                          "My Subject" + Environment.NewLine);
            Assert.Throws<InputException>(delegate { new EmailMessage(rawMessage); });

            rawMessage = new RawMessage("E123456789", "test@test.com" + Environment.NewLine +
                                                      "My Subject" + Environment.NewLine +
                                                      "");
            var ex = Assert.Throws<InputException>(delegate { new EmailMessage(rawMessage); });
            Assert.AreEqual(ex.Message, "The message text cannot be empty.");
        }

        [Test]
        public void MessageTextTooLongTest()
        {
            var rawMessage = new RawMessage("E123456789", "test@test.com" + Environment.NewLine +
                                                          "My Subject" + Environment.NewLine +
                                                          RandomString(1100));
            var ex = Assert.Throws<InputException>(delegate { new EmailMessage(rawMessage); });
            Assert.AreEqual(ex.Message, "Email message text cannot be longer than 1028 characters.");
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