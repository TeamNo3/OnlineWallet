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

        /// <summary>
        /// Пополняет указанный счёт на указанную сумму
        /// </summary>
        /// <param name="request"></param>
        /// <response code="200">Successful operation</response>
        /// <response code="202">Счёт заморожен</response>
        /// <response code="205">Номер счёта не найден</response>
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

        /// <summary>
        /// Выполняет перевод на указанную сумму
        /// </summary>
        /// <param name="request"></param>
        /// <response code="200">Successful operation</response>
        /// <response code="202">Один из счетов заморожен</response>
        /// <response code="205">Один из номеров счёта не найден</response>
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
