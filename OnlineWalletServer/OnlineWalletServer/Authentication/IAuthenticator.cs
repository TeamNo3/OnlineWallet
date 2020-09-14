using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace OnlineWalletServer.Authentication
{
    public interface IAuthenticator
    {
        Task Authenticate(string userName, HttpContext context);

        Task Logout(HttpContext context);
    }
}
