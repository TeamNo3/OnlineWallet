using System.Threading.Tasks;

namespace OnlineWalletServer.Authenticator
{
    public interface IAuthenticator
    {
        Task Authenticate(string userName);

        Task Logout();
    }
}
