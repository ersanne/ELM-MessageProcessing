using System;
using Newtonsoft.Json;
using SET09102_SoftwareEngineering_CW.exceptions;

namespace SET09102_SoftwareEngineering_CW.bdo
{
    /// <summary>
    /// Base message class, contains Id, Sender and MessageText
    /// </summary>
    public class Message
    {
        private string _id;

        public Message(RawMessage rawMessage)
        {
            //Header should be a letter followed by 9 digits thus should not be longer than 10 chars
            if (rawMessage.Header.Length != 10)
            {
                throw new InputException(
                    "Message header length is not valid, please make sure your id is correct.");
            }
        }

        [JsonProperty("id")]
        public string Id
        {
            get => _id;
            set
            {
                try
                {
                    //Try parsing the numeric part, will throw an exception if invalid
                    int.Parse(value
                        .Substring(1, 9));
                }
                catch
                {
                    //ID is not numeric
                    throw new InputException("ID in the message header is not valid.");
                }

                _id = value;
            }
        }

        [JsonProperty("sender")] public virtual string Sender { get; set; }
        [JsonProperty("messageText")] public virtual string MessageText { get; set; }

        //Serialize object to Json using Newtonsoft.Json
        public string ToIndentedJson()
        {
            
            var settings = new JsonSerializerSettings {NullValueHandling = NullValueHandling.Ignore, Formatting = Formatting.Indented};
            return JsonConvert.SerializeObject(this, settings);
        }
    }
}