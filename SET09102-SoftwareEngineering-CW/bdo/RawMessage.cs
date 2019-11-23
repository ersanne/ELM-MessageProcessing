using System;

namespace SET09102_SoftwareEngineering_CW.bdo
{
    public class RawMessage
    {
        public RawMessage(string header, string messageBody)
        {
            Header = header;
            MessageBody = messageBody;
        }

        public string Header { get; set; }

        public string MessageBody { get; set; }
    }
}