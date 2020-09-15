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

        /// <summary>
        /// Авторизация администратора
        /// </summary>
        /// <param name="signInRequestAdmin"></param>
        /// <response code="200">Successful operation</response>
        /// <response code="202">Неверный пароль</response>
        /// <response code="205">Администратор не найден</response>
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] SignInRequestAdmin signInRequestAdmin)
        {
            if (!ModelState.IsValid) return new BadRequestResult();

            var admin = await _dbContext.Administrator.FirstOrDefaultAsync(a => a.Username == signInRequestAdmin.Login || a.Email == signInRequestAdmin.Login);
            if (admin == null) return StatusCode(205);

            if (admin.Password != signInRequestAdmin.Password) return StatusCode(202);
            await Authenticate(admin.Username); // аутентификация

            return new OkResult();
        }

        /// <summary>
        /// Замораживает указанный номер счёта
        /// </summary>
        /// <param name="freezeRequest"></param>
        /// <response code="200">Successful operation</response>
        /// <response code="205">Номер счёта не найден</response>
        [HttpPost]
        [Route("freeze")]
        public async Task<IActionResult> Freeze([FromBody] FreezeRequest freezeRequest)
        {
            if (!ModelState.IsValid) return new BadRequestResult();

            var account = await _dbContext.Account.FirstOrDefaultAsync(a => a.Id == freezeRequest.Account);
            if (account == null) return StatusCode(205);
            account.IsFrozen = true;
            await _dbContext.SaveChangesAsync();
            return new OkResult();
        }

        /// <summary>
        /// Размораживает указанный номер счёта
        /// </summary>
        /// <param name="freezeRequest"></param>
        /// <returns></returns>
        /// <response code="200">Successful operation</response>
        /// <response code="205">Номер счёта не найден</response>
        [HttpPost]
        [Route("unfreeze")]
        public async Task<IActionResult> UnFreeze([FromBody] FreezeRequest freezeRequest)
        {
            if (!ModelState.IsValid) return new BadRequestResult();

            var account = await _dbContext.Account.FirstOrDefaultAsync(a => a.Id == freezeRequest.Account);
            if (account == null) return StatusCode(205);
            account.IsFrozen = false;
            await _dbContext.SaveChangesAsync();
            return new OkResult();
        }

        /// <summary>
        /// Отменяет перевод
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <response code="200">Successful operation</response>
        /// <response code="202">Один из счетов заморожен</response>
        /// <response code="204">Транзакция не найдена</response>
        /// <response code="205">Один из номеров счёта не найден</response>
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
