using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Database;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineWalletServer.Requests.Users;

namespace OnlineWalletServer.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly WalletDbContext _dbContext;

        public UsersController(WalletDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        [HttpPost]
        [Route("signup")]
        public async Task<IActionResult> Register([FromBody] SignUpRequest request)
        {
            if (!ModelState.IsValid) return new BadRequestResult();

            var user = await _dbContext.User.FirstOrDefaultAsync(u =>
                u.Email == request.Email || u.Username == request.Username);

            if (user != null) return StatusCode(409);

            // добавляем пользователя в бд
            await _dbContext.User.AddAsync(new User
            {
                Username = request.Username, Firstname = request.Firstname, Middlename = request.Middlename,
                Lastname = request.Lastname,
                Email = request.Email, Password = request.Password
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

            var user = await _dbContext.User.FirstOrDefaultAsync(u => (u.Username == request.Login || u.Email == request.Login) && u.Password == request.Password);
            if (user == null) return new UnauthorizedResult();

            await Authenticate(user.Username); // аутентификация

            return new OkResult();

        }

        private async Task Authenticate(string userName)
        {
            // создаем один claim
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userName)
            };
            // создаем объект ClaimsIdentity
            var id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            // установка аутентификационных куки
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

        [Route("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Users");
        }
    }
}
