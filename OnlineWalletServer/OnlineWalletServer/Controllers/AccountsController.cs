using System.Threading.Tasks;
using Database;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize]
        public async Task Add([FromBody] AddRequest request)
        {

        }

        [HttpPost]
        [Route("transfer")]
        [Authorize]
        public async Task Transfer([FromBody] TransferRequest request)
        {

        }
    }
}
