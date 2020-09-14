using System;
using System.Threading.Tasks;
using Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineWalletServer.Requests.Accounts;
using Services.Transactions;
using Services.Transactions.Exceptions;

namespace OnlineWalletServer.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly WalletDbContext _dbContext;

        private readonly ITransactionPerformer _transaction;

        public AccountsController(WalletDbContext dbContext, ITransactionPerformer transaction)
        {
            _dbContext = dbContext;
            _transaction = transaction;
        }

        [HttpPost]
        [Route("add")]
        [Authorize]
        public async Task<IActionResult> Add([FromBody] AddRequest request)
        {
            var account = await _dbContext.Account.FirstOrDefaultAsync(a => a.Id == request.Account);
            if (account == null) return StatusCode(205);
            if (account.IsFrozen) return StatusCode(202);
            account.Balance += request.MoneyAmount;
            await _dbContext.SaveChangesAsync();
            return new OkResult();
        }

        [HttpPost]
        [Route("transfer")]
        [Authorize]
        public async Task<IActionResult> Transfer([FromBody] TransferRequest request)
        {
            var transaction = new Transaction
                {Amount = request.MoneyAmount, Datetime = DateTime.Now, From = request.From, To = request.To};
            try
            {
                await _transaction.PerformAsync(transaction);
            }
            catch (AccountsNotFoundException e)
            {
                return StatusCode(205);
            }
            catch (AccountsFrozenException e)
            {
                return StatusCode(202);
            }

            await _dbContext.Transaction.AddAsync(transaction);
            return new OkResult();
        }
    }
}
