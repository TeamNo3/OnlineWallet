using System;
using System.Threading.Tasks;
using Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineWalletServer.Requests.Admin;
using Services.Transactions;

namespace OnlineWalletServer.Controllers
{
    [Route("api/admin")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly WalletDbContext _dbContext;

        public ITransactionPerformer Transaction { get; set; }

        public AdminController(WalletDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        [HttpPost]
        [Route("freeze")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Freeze([FromBody] FreezeRequest request)
        {
            if (!ModelState.IsValid) return new BadRequestResult();

            var account = await _dbContext.Account.FirstOrDefaultAsync(a => a.Id == request.Account);
            if (account == null) return new BadRequestResult();
            account.IsFrozen = true;
            await _dbContext.SaveChangesAsync();
            return new OkResult();
        }

        [HttpPost]
        [Route("cancel")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Cancel([FromBody] TransactionCancelRequest request)
        {
            if (!ModelState.IsValid) return new BadRequestResult();

            var transactionToCancel = await _dbContext.Transaction.FirstOrDefaultAsync(t => t.Id == request.TransactionId);
            if (transactionToCancel == null) return new BadRequestResult();

            var newTransaction = new Transaction
            {
                Amount = transactionToCancel.Amount, From = transactionToCancel.To, To = transactionToCancel.From,
                Datetime = DateTime.Now
            };

            await Transaction.PerformAsync(newTransaction);
            await _dbContext.Transaction.AddAsync(newTransaction);

            return new OkResult();
        }
    }
}
