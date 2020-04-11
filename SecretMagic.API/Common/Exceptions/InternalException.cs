namespace SecretMagic.API.Commom
{
    public class InternalException : System.Exception
    {
        public InternalException() { }
        public InternalException(string message) : base(message) { }
        public InternalException(string message, System.Exception inner) : base(message, inner) { }
        protected InternalException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}