using System;
using System.Threading.Tasks;
using Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineWalletServer.Authentication;
using OnlineWalletServer.Requests.Admin;
using Services.Transactions;
using Services.Transactions.Exceptions;

namespace OnlineWalletServer.Controllers
{
    [Route("api/admin")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly WalletDbContext _dbContext;

        private readonly IAuthenticator _authenticator;

        private readonly ITransactionPerformer _transaction;

        public AdminController(WalletDbContext dbContext, IAuthenticator authenticator, ITransactionPerformer transaction)
        {
            _dbContext = dbContext;
            _authenticator = authenticator;
            _transaction = transaction;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] SignInRequestAdmin request)
        {
            if (!ModelState.IsValid) return new BadRequestResult();

            var admin = await _dbContext.Administrator.FirstOrDefaultAsync(a => (a.Username == request.Login || a.Email == request.Login) && a.Password == request.Password);
            if (admin == null) return StatusCode(205);

            await Authenticate(admin.Username); // аутентификация

            return new OkResult();
        }

        [HttpPost]
        [Route("freeze")]
        public async Task<IActionResult> Freeze([FromBody] FreezeRequest request)
        {
            if (!ModelState.IsValid) return new BadRequestResult();

            var account = await _dbContext.Account.FirstOrDefaultAsync(a => a.Id == request.Account);
            if (account == null) return StatusCode(205);
            account.IsFrozen = true;
            await _dbContext.SaveChangesAsync();
            return new OkResult();
        }

        [HttpPost]
        [Route("unfreeze")]
        public async Task<IActionResult> UnFreeze([FromBody] FreezeRequest request)
        {
            if (!ModelState.IsValid) return new BadRequestResult();

            var account = await _dbContext.Account.FirstOrDefaultAsync(a => a.Id == request.Account);
            if (account == null) return StatusCode(205);
            account.IsFrozen = false;
            await _dbContext.SaveChangesAsync();
            return new OkResult();
        }

        [HttpPost]
        [Route("cancel")]
        public async Task<IActionResult> Cancel([FromBody] TransactionCancelRequest request)
        {
            if (!ModelState.IsValid) return new BadRequestResult();

            var transactionToCancel = await _dbContext.Transaction.FirstOrDefaultAsync(t => t.Id == request.TransactionId);
            if (transactionToCancel == null) return StatusCode(205);

            var newTransaction = new Transaction
            {
                Amount = transactionToCancel.Amount, From = transactionToCancel.To, To = transactionToCancel.From,
                Datetime = DateTime.Now
            };

            try
            {
                await _transaction.PerformAsync(newTransaction);
            }
            catch (AccountsNotFoundException e)
            {
                return StatusCode(205);
            }
            catch (AccountsFrozenException e)
            {
                return StatusCode(202);
            }

            await _dbContext.Transaction.AddAsync(newTransaction);

            return new OkResult();
        }

        private async Task Authenticate(string userName)
        {
            await _authenticator.Authenticate(userName, HttpContext);
        }

        [HttpPost]
        [Route("logout")]
        public async Task<IActionResult> Logout()
        {
            await _authenticator.Logout(HttpContext);
            return RedirectToAction("Login", "Admin");
        }
    }
}
