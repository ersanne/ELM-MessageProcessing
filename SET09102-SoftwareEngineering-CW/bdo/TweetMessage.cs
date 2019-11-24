using System;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SET09102_SoftwareEngineering_CW.exceptions;

namespace SET09102_SoftwareEngineering_CW.bdo
{
    public class TweetMessage : Message
    {
        public TweetMessage(RawMessage message) : base(message)
        {
            var endOfFirstLine = message.MessageBody.IndexOf("\n", StringComparison.Ordinal)+1;
            Sender = message.MessageBody.Substring(0, endOfFirstLine-1).Trim();
        }

        [JsonProperty("sender")]
        public sealed override string Sender
        {
            get => base.Sender;
            set
            {
                if (!IsValidSender(value))
                {
                    throw new InputException("Tweet sender is not valid.");
                }
                base.Sender = value;
            }
        }

        private static bool IsValidSender(string sender)
        {
            try
            {
                return Regex.Match(sender, @"\@.{0,15}$").Success;
            }
            catch
            {
                return false;
            }
            
        }
    }
}