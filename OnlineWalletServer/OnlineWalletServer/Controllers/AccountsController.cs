using System.Threading.Tasks;
using Database;
using Microsoft.AspNetCore.Mvc;
using OnlineWalletServer.Requests.Accounts;

namespace OnlineWalletServer.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private WalletDbContext dbContext;

        public AccountsController(WalletDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpPost]
        [Route("add")]
        public async Task Post([FromBody] AddRequest request)
        {

        }

        [HttpPost]
        [Route("transfer")]
        public async Task Post([FromBody] TransferRequest request)
        {

        }
    }
}
