using System;
using System.Text.RegularExpressions;
using ELMPrototype.Tests.exceptions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace ELMPrototype.Tests.bdo
{
    /// <summary>
    /// Tweet inherits message and customizes verification
    /// </summary>
    public class TweetMessage : Message
    {
        public TweetMessage()
        {
        }

        public TweetMessage(RawMessage message) : base(message)
        {
            //Find end of first line to avoid splitting whole text by lines
            var endOfFirstLine = message.MessageBody.IndexOf("\n", StringComparison.Ordinal);

            //Get sender line and set property
            Sender = message.MessageBody.Substring(0, endOfFirstLine).Trim();

            //Set MessageText
            MessageText = message.MessageBody.Substring(endOfFirstLine + 1);
        }

        [JsonProperty("sender")]
        public sealed override string Sender
        {
            get => base.Sender;
            set
            {
                //Check that sender is a valid twitter handle
                if (!IsValidSender(value))
                {
                    throw new InputException("Tweet sender is not valid.");
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
                {
                    //Check that message text is not empty
                    if (string.IsNullOrEmpty(value))
                    {
                        throw new InputException("The message text cannot be empty.");
                    }

                    //Check that message text is no longer 140 characters
                    if (value.Length > 140)
                    {
                        throw new InputException("Tweet message text cannot be longer than 140 characters.");
                    }

                    base.MessageText = value;
                }
            }
        }

        private static bool IsValidSender(string sender)
        {
            try
            {
                //Regex to match twitter handle, throws exception if no match
                return Regex.Match(sender, @"\@.{0,15}$").Success;
            }
            catch
            {
                return false;
            }
        }
    }
}