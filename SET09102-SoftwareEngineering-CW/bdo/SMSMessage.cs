using System;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using SET09102_SoftwareEngineering_CW.exceptions;

namespace SET09102_SoftwareEngineering_CW.bdo
{
    public class SMSMessage : Message
    {
        public SMSMessage(RawMessage rawMessage) : base(rawMessage)
        {
            var endOfFirstLine = rawMessage.MessageBody.IndexOf("\n", StringComparison.Ordinal);
            Sender = rawMessage.MessageBody.Substring(0, endOfFirstLine - 1); //Phone number cant be longer than 15 characters
            var body = rawMessage.MessageBody.Substring(endOfFirstLine + 1);
            MessageText = body;
        }

        [JsonProperty("sender")]
        public sealed override string Sender
        {
            get => base.Sender;
            set
            {
                if (!IsPhoneNumber(value))
                {
                    throw new InputException("Phone number \"" + value +
                                             "\" is not valid. Please enter a correct phone number.");
                }

                base.Sender = value;
            }
        }

        [JsonProperty("messageText")]
        public sealed override string MessageText
        {
            get => base.MessageText;
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new InputException("The message text cannot be empty.");
                }

                if (value.Length > 140)
                {
                    throw new InputException("SMS message text cannot be longer than 140 characters.");
                }

                base.MessageText = value;
            }
        }

        private static bool IsPhoneNumber(string number)
        {
            return Regex.Match(number, @"^\+(?:[0-9]●?){6,14}[0-9]$").Success;
        }
    }
}