using System.ComponentModel.DataAnnotations;

namespace OnlineWalletServer.Requests.Users
{
    public class SignInRequest
    {
        [Required]
        public string Login { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } 
    }
}
