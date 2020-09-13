using System.Threading.Tasks;
using Database;
using Microsoft.AspNetCore.Mvc;
using OnlineWalletServer.Requests.Users;

namespace OnlineWalletServer.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private WalletDbContext dbContext;

        public UsersController(WalletDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpPost]
        [Route("register")]
        public async Task Post([FromBody] SignUpRequest request)
        {

        }

        [HttpPost]
        [Route("confirm/{userId}")]
        public async Task Post([FromRoute] int userId)
        {

        }

        [HttpPost]
        [Route("authenticate")]
        public async Task Post([FromBody] SignInRequest request)
        {

        }
    }
}
