namespace OnlineWalletServer.Requests.Users
{
    public class SignUpRequest
    {
        public string Username { get; set; }

        public string Firstname { get; set; }

        public string Middlename { get; set; }

        public string Lastname { get; set; }
        
        public string Email { get; set; }
        
        public string Password { get; set; }
    }
}
