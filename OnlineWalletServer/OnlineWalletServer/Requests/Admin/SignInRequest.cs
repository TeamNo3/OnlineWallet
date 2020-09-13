using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineWalletServer.Requests.Admin
{
    public class SignInRequest
    {
        public string Login { get; set; }

        public string Password { get; set; }
    }
}
