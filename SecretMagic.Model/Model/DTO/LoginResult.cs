namespace SecretMagic.Model
{
    public class LoginResult
    {
        public string Token { get; set; }

        public User User { get; set; }

        public string Role{ get;set; }

        public string[] Resource { get;set; }
    }
}