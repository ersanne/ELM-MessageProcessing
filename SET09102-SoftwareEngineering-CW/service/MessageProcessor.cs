using System;
using System.Text.RegularExpressions;
using SET09102_SoftwareEngineering_CW.bdo;

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

            return null;
        }

        private string ProcessSMSMessage(RawMessage message)
        {
            foreach (var word in basicDataProvider.TextSpeakWords)
            {
                if (message.Body.Contains(word.Key))
                {
                    message.Body = message.Body.Replace(word.Key, "<"+ word.Value + ">");
                }
            }
            var parsed = new SMSMessage(message);
            return parsed.ToJson();
        }

        private string ProcessEmailMessage(RawMessage message)
        {
            return "";
        }

        private string ProcessTweetMessage(RawMessage message)
        {
            return "";
        }

    }
}