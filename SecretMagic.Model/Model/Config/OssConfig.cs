namespace SecretMagic.Model
{
    public class OssConfig
    {
        public string Region { get; set; }
        public string OssRegion { get; set; }
        public string StsEndPoint { get; set; }
        public string OssEndPoint { get; set; }
        public string AccessKeyId { get; set; }
        public string AccessKeySecret { get; set; }
        public string Bucket { get; set; }
        public int Expiration { get; set; }
        public string RoleArn { get; set; }
        public string RoleSessionName { get; set; }
    }
}