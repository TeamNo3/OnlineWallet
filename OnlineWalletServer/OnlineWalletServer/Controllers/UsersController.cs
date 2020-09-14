using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Database;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
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

        [HttpPost]
        [Route("signup")]
        public async Task<IActionResult> Register([FromBody] SignUpRequest request)
        {
            if (!ModelState.IsValid) return new BadRequestResult();

            var user = await _dbContext.User.FirstOrDefaultAsync(u =>
                u.Email == request.Email || u.Username == request.Username);

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
                Username = request.Username, Firstname = request.Firstname, Middlename = request.Middlename,
                Lastname = request.Lastname,
                Email = request.Email, Password = request.Password, Account = accountId
            });

            await _dbContext.SaveChangesAsync();

            await Authenticate(request.Username); // аутентификация

            return new OkResult();

        }

        [HttpPost]
        [Route("confirm/{userId}")]
        public async Task<IActionResult> Confirm([FromRoute] int userId)
        {
            var user = await _dbContext.User.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null) return new BadRequestResult();

            user.IsConfirmed = true;
            await _dbContext.SaveChangesAsync();
            return new OkResult();
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] SignInRequest request)
        {
            if (!ModelState.IsValid) return new BadRequestResult();

            var user = await _dbContext.User.FirstOrDefaultAsync(u =>
                (u.Username == request.Login || u.Email == request.Login) && u.Password == request.Password);
            if (user == null) return StatusCode(205);

            await Authenticate(user.Username); // аутентификация

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
