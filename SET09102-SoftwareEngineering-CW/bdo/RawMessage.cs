using System;

namespace SET09102_SoftwareEngineering_CW.bdo
{
    public class RawMessage
    {
        private String header;
        private String body;

        public RawMessage(string header, string body)
        {
            this.header = header;
            this.body = body;
        }

        public string Header
        {
            get => header;
            set => header = value;
        }

        public string Body
        {
            get => body;
            set => body = value;
        }
    }
}