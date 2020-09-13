using System;
using System.Threading.Tasks;
using Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineWalletServer.Requests.Accounts;
using Services.Transactions;

namespace OnlineWalletServer.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly WalletDbContext _dbContext;

        public ITransactionPerformer Transaction { get; set; }

        public AccountsController(WalletDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost]
        [Route("add")]
        [Authorize]
        public async Task<IActionResult> Add([FromBody] AddRequest request)
        {
            var account = await _dbContext.Account.FirstOrDefaultAsync(a => a.Id == request.Account);
            if (account == null) return new BadRequestResult();
            account.Balance += request.MoneyAmount;
            await _dbContext.SaveChangesAsync();
            return new OkResult();
        }

        [HttpPost]
        [Route("transfer")]
        [Authorize]
        public async Task Transfer([FromBody] TransferRequest request)
        {
            var transaction = new Transaction
                {Amount = request.MoneyAmount, Datetime = DateTime.Now, From = request.From, To = request.To};
            await Transaction.PerformAsync(transaction);
            await _dbContext.Transaction.AddAsync(transaction);
        }
    }
}
