using System;
using Newtonsoft.Json;

namespace ELMPrototype.bdo
{
    /// <summary>
    /// Raw message with Header and MessageBody
    /// </summary>
    public class RawMessage
    {
        public RawMessage(string header, string messageBody)
        {
            Header = header;
            MessageBody = messageBody;
        }

        public string Header { get; }

        public string MessageBody { get; set; }
    }
}