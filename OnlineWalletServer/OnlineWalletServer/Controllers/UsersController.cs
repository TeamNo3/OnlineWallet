using System;
using System.Threading.Tasks;
using Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineWalletServer.Authentication;
using OnlineWalletServer.Requests.Users;

namespace OnlineWalletServer.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly WalletDbContext _dbContext;

        private readonly IAuthenticator _authenticator;

        public UsersController(WalletDbContext dbContext, IAuthenticator authenticator)
        {
            _dbContext = dbContext;
            _authenticator = authenticator;
        }

        /// <summary>
        /// Регистрирует нового пользователя
        /// </summary>
        /// <param name="signUpRequest"></param>
        /// <response code="200">Successful operation</response>
        /// <response code="205">Пользователь с таким именем или email-адресом уже существует</response>
        [HttpPost]
        [Route("signup")]
        public async Task<IActionResult> Register([FromBody] SignUpRequest signUpRequest)
        {
            if (!ModelState.IsValid) return new BadRequestResult();

            var user = await _dbContext.User.FirstOrDefaultAsync(u =>
                u.Email == signUpRequest.Email || u.Username == signUpRequest.Username);

            if (user != null) return StatusCode(205);

            var accountId = Guid.NewGuid().ToString();

            await _dbContext.Account.AddAsync(new Account
            {
                Id = accountId,
                Balance = 0,
                IsFrozen = false
            });

            await _dbContext.User.AddAsync(new User
            {
                Username = signUpRequest.Username, Firstname = signUpRequest.Firstname, Middlename = signUpRequest.Middlename,
                Lastname = signUpRequest.Lastname,
                Email = signUpRequest.Email, Password = signUpRequest.Password, Account = accountId
            });

            await _dbContext.SaveChangesAsync();

            await Authenticate(signUpRequest.Username); // аутентификация

            return new OkResult();

        }

        /// <summary>
        /// Подтверждение указанного пользователя
        /// </summary>
        /// <param name="userId"></param>
        /// <response code="200">Successful operation</response>
        /// <response code="205">Пользователь не найден</response>
        [HttpPost]
        [Route("confirm/{userId}")]
        public async Task<IActionResult> Confirm([FromRoute] int userId)
        {
            var user = await _dbContext.User.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null) return StatusCode(205);

            user.IsConfirmed = true;
            await _dbContext.SaveChangesAsync();
            return new OkResult();
        }

        /// <summary>
        /// Авторизация пользователя
        /// </summary>
        /// <param name="signInRequest"></param>
        /// <response code="200">Successful operation</response>
        /// <response code="202">Неверный пароль</response>
        /// <response code="205">Пользователь не найден</response>
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] SignInRequest signInRequest)
        {
            if (!ModelState.IsValid) return new BadRequestResult();

            var user = await _dbContext.User.FirstOrDefaultAsync(u => u.Username == signInRequest.Login || u.Email == signInRequest.Login);
            if (user == null) return StatusCode(205);

            if (user.Password != signInRequest.Password) return StatusCode(202);

            await Authenticate(user.Username);

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
            return RedirectToAction("Login", "Users");
        }
    }
}
