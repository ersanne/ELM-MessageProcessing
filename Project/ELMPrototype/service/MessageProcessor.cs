using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ELMPrototype.bdo;
using ELMPrototype.exceptions;

namespace ELMPrototype.service
{
    /// <summary>
    ///     Singleton class to allow processing of messages
    ///     Requires BasicDataProvider
    /// </summary>
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

        /// <summary>
        ///     Process the raw message and return a formatted/indented json.
        ///     This could be more customized but this is enough for the prototype.
        /// </summary>
        /// <param name="rawMessage">The input message</param>
        /// <returns>The parsed message as json</returns>
        /// <exception cref="InputException">The input was invalid</exception>
        public string ProcessMessageIndentedJson(RawMessage rawMessage)
        {
            switch (char.ToUpper(rawMessage.Header[0])) //Not clear whether lower case type is allowed
            {
                case 'S': //SMS
                    return ProcessSmsMessage(rawMessage).ToIndentedJson();
                case 'E': //Email
                    return ProcessEmailMessage(rawMessage).ToIndentedJson();
                case 'T': //Tweet
                    return ProcessTweetMessage(rawMessage).ToIndentedJson();
            }

            //If message type could not be detected throw exception
            throw new InputException(
                "Could not detect message type. Please ensure the message is correctly formatted.");
        }

        private SMSMessage ProcessSmsMessage(RawMessage message)
        {
            //Parse message before transforming text. Verifies any length etc. requirements
            var parsed = new SMSMessage(message);

            //Expand textspeak
            parsed.MessageText = ExpandTextspeak(parsed.MessageText);

            //Return message
            return parsed;
        }

        private EmailMessage ProcessEmailMessage(RawMessage message)
        {
            //Parse message before transforming text. Verifies any length etc. requirements
            var parsed = new EmailMessage(message);

            //If message has been detected as SIR add it to the list
            if (parsed.IsSir)
                _basicDataProvider.SirList.AddIfAbsent(new SirItem(parsed.SportCentreCode, parsed.IncidentType));

            //Quarantine links | Regex from https://docs.microsoft.com/en-us/previous-versions/msp-n-p/ff650303(v=pandp.10)?redirectedfrom=MSDN#paght000001_commonregularexpressions
            var linkParser = new Regex(
                @"(ht|f)tp(s?)\:\/\/[0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*(:(0-9)*)*(\/?)([a-zA-Z0-9\-\.\?\,\'\/\\\+&amp;%\$#_]*)?",
                RegexOptions.Compiled | RegexOptions.IgnoreCase);
            foreach (Match m in linkParser.Matches(parsed.MessageText))
            {
                //Add to quarantine list if not in the list already
                if (!_basicDataProvider.QuarantineList.Contains(m.Value))
                    _basicDataProvider.QuarantineList.Add(m.Value);

                //Replace url with <URL Quarantined>
                parsed.MessageText = parsed.MessageText.Replace(m.Value, "<URL Quarantined>");
            }

            //Return message
            return parsed;
        }

        private TweetMessage ProcessTweetMessage(RawMessage message)
        {
            //Parse message before transforming text. Verifies any length etc. requirements
            var parsed = new TweetMessage(message);

            //Expand textspeak
            parsed.MessageText = ExpandTextspeak(parsed.MessageText);

            //Add/Increment hashtags to/in trending list
            var hashtags = new HashSet<string>(); //Keep track of hashtags to avoid double counting
            var hashtagRegex = new Regex(@"\#\w+");
            foreach (Match m in hashtagRegex.Matches(parsed.MessageText))
                //Only process if first occurence in tweet
                if (!hashtags.Contains(m.Value))
                {
                    _basicDataProvider.TrendingList.AddOrIncrement(m.Value);
                    hashtags.Add(m.Value);
                }

            //Add mention to mention list
            var mentionRegex = new Regex(@"\@\w+");
            foreach (Match m in mentionRegex.Matches(parsed.MessageText))
                //Ignore if mention already exists in list
                if (!_basicDataProvider.MentionList.Any(item => item.Equals(m.Value)))
                    _basicDataProvider.MentionList.Add(m.Value);

            //Return message
            return parsed;
        }

        private string ExpandTextspeak(string text)
        {
            //Loop through textspeak words
            foreach (var word in _basicDataProvider.TextSpeakWords)
                //Replace all occurrences of textspeak word
                text = text.Replace(word.Key, $"{word.Key} <{word.Value}>");

            //Return text with expanded textspeak
            return text;
        }
    }
}