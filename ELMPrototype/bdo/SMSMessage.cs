using System;
using System.Text.RegularExpressions;
using ELMPrototype.exceptions;
using Newtonsoft.Json;

namespace ELMPrototype.bdo
{
    /// <summary>
    /// SMS inherits message and customizes verification
    /// </summary>
    public class SMSMessage : Message
    {
        public SMSMessage()
        {
        }

        public SMSMessage(RawMessage rawMessage) : base(rawMessage)
        {
            //Find end of first line to avoid splitting whole text by lines
            var endOfFirstLine = rawMessage.MessageBody.IndexOf("\n", StringComparison.Ordinal);

            //Get sender line and set property
            Sender = rawMessage.MessageBody.Substring(0, endOfFirstLine - 1);

            //Set MessageText
            MessageText = rawMessage.MessageBody.Substring(endOfFirstLine + 1);
        }

        [JsonProperty("sender")]
        public sealed override string Sender
        {
            get => base.Sender;
            set
            {
                //Verify that sender is a valid international phone number
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
                //Check that message text is not empty
                if (string.IsNullOrEmpty(value))
                {
                    throw new InputException("The message text cannot be empty.");
                }

                //Check that MessageText is no longer than 140 characters
                if (value.Length > 140)
                {
                    throw new InputException("SMS message text cannot be longer than 140 characters.");
                }

                base.MessageText = value;
            }
        }

        private static bool IsPhoneNumber(string number)
        {
            try
            {
                //Regex to match international phone number, throws exception if no match
                return Regex.Match(number, @"^\+(?:[0-9]●?){6,14}[0-9]$").Success;
            }
            catch
            {
                return false;
            }
        }
    }
}