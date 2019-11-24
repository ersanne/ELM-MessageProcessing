using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using SET09102_SoftwareEngineering_CW.bdo;
using SET09102_SoftwareEngineering_CW.exceptions;

namespace SET09102_SoftwareEngineering_CW.service
{
    public class MessageProcessor
    {
        private static MessageProcessor _instance;
        private readonly BasicDataProvider _basicDataProvider;

        private MessageProcessor()
        {
            _basicDataProvider = BasicDataProvider.GetInstance();
        }

        public static MessageProcessor GetInstance()
        {
            return _instance ?? (_instance = new MessageProcessor());
        }

        public string ProcessMessage(RawMessage rawMessage)
        {
            switch (rawMessage.Header[0])
            {
                case 'S':
                    return ProcessSmsMessage(rawMessage);
                case 'E':
                    return ProcessEmailMessage(rawMessage);
                case 'T':
                    return ProcessTweetMessage(rawMessage);
            }
            throw new InputException("Could not detect message type. Please ensure the message is correctly formatted.");
        }

        private string ProcessSmsMessage(RawMessage message)
        {
            ExpandTextspeak(message);
            var parsed = new SMSMessage(message);
            return parsed.ToJson();
        }

        private string ProcessEmailMessage(RawMessage message)
        {
            var parsed = new EmailMessage(message);
            if (parsed.IsSir)
            {
                _basicDataProvider.SirList.AddIfAbsent(new SirItem(parsed.SportCentreCode, parsed.IncidentType));
            }
            
            var linkParser = new Regex(
                @"\b(?:https?://|www\.)\S+\b",
                RegexOptions.Compiled | RegexOptions.IgnoreCase);
            foreach (Match m in linkParser.Matches(parsed.MessageText))
            {
                if (!_basicDataProvider.QuarantineList.Contains(m.Value))
                {
                    _basicDataProvider.QuarantineList.Add(m.Value);   
                }

                parsed.MessageText = parsed.MessageText.Replace(m.Value, "<URL Quarantined>");
            }
            
            return parsed.ToJson();
        }

        private string ProcessTweetMessage(RawMessage message)
        {
            ExpandTextspeak(message);
            
            var hashtags = new HashSet<string>(); //Keep track of hashtags to avoid double counting
            var hashtagRegex = new Regex(@"\#\w+");
            foreach (Match m in hashtagRegex.Matches(message.MessageBody))
            {
                if (!hashtags.Contains(m.Value))
                {
                    _basicDataProvider.AddOrIncrementTrendingListItem(m.Value);
                    hashtags.Add(m.Value);
                }
                
            }

            var mentionRegex = new Regex(@"\@\w+");
            var endOfFirstLine = message.MessageBody.IndexOf("\n", StringComparison.Ordinal)+1; //Exclude the Sender
            foreach (Match m in mentionRegex.Matches(message.MessageBody.Substring(endOfFirstLine)))
            {
                if (!_basicDataProvider.MentionList.Any(item => item.Equals(m.Value)))
                    _basicDataProvider.MentionList.Add(m.Value);
            }

            return new TweetMessage(message).ToJson();
        }

        private void ExpandTextspeak(RawMessage message)
        {
            foreach (var word in _basicDataProvider.TextSpeakWords)
            {
                if (message.MessageBody.Contains(word.Key))
                {
                    message.MessageBody = message.MessageBody.Replace(word.Key, $"{word.Key} <{word.Value}>");
                }
            }
        }
    }
}