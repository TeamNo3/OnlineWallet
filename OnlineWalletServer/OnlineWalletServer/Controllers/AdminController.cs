using System.Threading.Tasks;
using Database;
using Microsoft.AspNetCore.Mvc;
using OnlineWalletServer.Requests.Admin;

namespace OnlineWalletServer.Controllers
{
    [Route("api/admin")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private WalletDbContext dbContext;

        public AdminController(WalletDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpPost]
        [Route("authenticate")]
        public async Task Post([FromBody] SignInRequest request)
        {

        }

        [HttpPost]
        [Route("freeze")]
        public async Task Post([FromBody] FreezeRequest request)
        {

        }

        [HttpPost]
        [Route("cancel")]
        public async Task Post([FromBody] TransactionCancelRequest request)
        {

        }
    }
}
