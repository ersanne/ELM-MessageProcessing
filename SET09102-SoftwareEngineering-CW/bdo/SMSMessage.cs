using System;

namespace SET09102_SoftwareEngineering_CW.bdo
{
    public class SMSMessage
    {
        private char type;
        private int id;
        private string sender;
        private string messageText;

        public SMSMessage(RawMessage message)
        {
            this.type = message.Header[0];
            this.id = Int32.Parse(message.Header.Substring(1, 1));
            this.sender = message.Body.Substring(0, 15);
            this.messageText = message.Body.Substring(1).Trim();
        }

        public string ToJson()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this);
        }

    }
}