using System.Linq;
using System.Text.RegularExpressions;
using SET09102_SoftwareEngineering_CW.bdo;
using SET09102_SoftwareEngineering_CW.exceptions;

namespace SET09102_SoftwareEngineering_CW.service
{
    public class MessageProcessor
    {
        private static MessageProcessor instance;
        private BasicDataProvider basicDataProvider;

        private MessageProcessor()
        {
            basicDataProvider = BasicDataProvider.GetInstance();
        }

        public static MessageProcessor GetInstance()
        {
            return instance ?? (instance = new MessageProcessor());
        }

        public string ProcessMessage(RawMessage rawMessage)
        {
            switch (rawMessage.Header[0])
            {
                case 'S':
                    return ProcessSMSMessage(rawMessage);
                case 'E':
                    return ProcessEmailMessage(rawMessage);
                case 'T':
                    return ProcessTweetMessage(rawMessage);
            }
            throw new InputException("Could not detect message type. Please ensure the message is correctly formatted.");
        }

        private string ProcessSMSMessage(RawMessage message)
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
                basicDataProvider.SirList.AddIfAbsent(new SirItem(parsed.SportCentreCode, parsed.IncidentType));
            }
            return parsed.ToJson();
        }

        private string ProcessTweetMessage(RawMessage message)
        {
            ExpandTextspeak(message);
            
            //^#\w+$ 
            var hashtagRegex = new Regex(@"(?<=#)\w+");
            foreach (Match m in hashtagRegex.Matches(message.MessageBody))
            {
                basicDataProvider.AddOrIncrementTrendingListItem(m.Value);
            }

            var mentionRegex = new Regex(@"(?<=\@)\w+");
            foreach (Match m in mentionRegex.Matches(message.MessageBody))
            {
                if (!basicDataProvider.MentionList.Any(item => item.Equals(m.Value)))
                    basicDataProvider.MentionList.Add(m.Value);
            }

            return new Message(message).ToJson();
        }

        private void ExpandTextspeak(RawMessage message)
        {
            foreach (var word in basicDataProvider.TextSpeakWords)
            {
                if (message.MessageBody.Contains(word.Key))
                {
                    message.MessageBody = message.MessageBody.Replace(word.Key, $"{word.Key} <{word.Value}>");
                }
            }
        }
    }
}