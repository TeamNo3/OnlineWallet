using System.ComponentModel.DataAnnotations;

namespace OnlineWalletServer.Requests.Admin
{
    public class SignInRequestAdmin
    {
        [Required]
        public string Login { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
