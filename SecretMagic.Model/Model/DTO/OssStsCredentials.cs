namespace SecretMagic.Model
{
    public class OssStsCredentials
    {
        public string Region { get; set; }

        public string Bucket { get; set; }

        public string AccessKeyId { get; set; }

        public string AccessKeySecret { get; set; }
        public string SecurityToken { get; set; }
        public string Expiration { get; set; }
    }
}