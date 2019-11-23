using System;
using Newtonsoft.Json;
using SET09102_SoftwareEngineering_CW.exceptions;

namespace SET09102_SoftwareEngineering_CW.bdo
{
    public class Message
    {
        [JsonProperty("Id")] private string _id;

        [field: JsonProperty("Sender")] public virtual string Sender { get; set; }
        [field: JsonProperty("MessageText")] public virtual string MessageText { get; set; }

        public Message(RawMessage rawMessage)
        {
            if (rawMessage.Header.Length != 10)
            {
                throw new InputException(
                    "Message header length is not valid, please make sure your id is correct."); //Invalid length, throw exception
            }
        }
        
        public string Id
        {
            get { return _id; }
            set
            {
                try
                {
                    int.Parse(value
                        .Substring(1, 9)); //Try parsing the numeric part, will throw an exception if invalid
                }
                catch
                {
                    throw new InputException("ID in the message header is not valid.");
                }

                _id = value;
            }
        }

        public string ToJson()
        {
            var settings = new JsonSerializerSettings();
            settings.NullValueHandling = NullValueHandling.Ignore;
            return JsonConvert.SerializeObject(this, settings);
        }
    }
}