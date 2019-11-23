namespace SET09102_SoftwareEngineering_CW.bdo
{
    public class TweetMessage : Message
    {
        public TweetMessage(RawMessage rawMessage, string sender) : base(rawMessage)
        {
            Sender = sender;
        }

        public sealed override string Sender
        {
            get => base.Sender;
            set => base.Sender = value;
        }
    }
}