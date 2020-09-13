using System.Threading.Tasks;
using Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineWalletServer.Requests.Admin;

namespace OnlineWalletServer.Controllers
{
    [Route("api/admin")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private WalletDbContext _dbContext;

        public AdminController(WalletDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        [HttpPost]
        [Route("freeze")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Freeze([FromBody] FreezeRequest request)
        {
            return new OkResult();
        }

        [HttpPost]
        [Route("cancel")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Cancel([FromBody] TransactionCancelRequest request)
        {
            return new OkResult();
        }
    }
}
