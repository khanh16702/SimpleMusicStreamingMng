using CoreLib.Config;
using CoreLib.DataTableToObject.Mapping;
using CoreLib.Entities;
using DataServiceLib.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Data;
using System.Security.Claims;

namespace SimpleMusicStreaming.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("admin/login")]
    public class LoginController : Controller
    {
        private readonly IAccountDataProvider _accountDataProvider;
        public IConfiguration Configuration { get; }

        private readonly IStringLocalizer<SharedResource> _sharedLocalizer;

        public LoginController(IAccountDataProvider accountDataProvider, IConfiguration configuration, IStringLocalizer<SharedResource> sharedLocalizer)
        {
            _accountDataProvider = accountDataProvider;
            Configuration = configuration;
            _sharedLocalizer = sharedLocalizer;
        }

        [Route(LinkRoute.Index)]
        public IActionResult Index()
        {
            return View();
        }

        [Route("SignIn")]
        [HttpPost]
        public async Task<IActionResult> SignIn(CAccount model)
        {
            CResponseMessage response = new CResponseMessage();
            response = _accountDataProvider.Search(model.Username);
            CAccount account = ((DataSet)response.Data).ToListItem<CAccount>().FirstOrDefault(x => x.Password == model.Password);
            if (account != null)
            {
                var claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.NameIdentifier, model.Username.ToLower())
                };

                // Chưa add role

                var claimIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimIdentity));

                return Redirect("/admin/home/index");
            }
            else
            {
                TempData["Error"] = "Login failed";
                return Redirect("/admin/login/index");
            }
        }
        [Route("SignOut")]
        public new async Task<IActionResult> SignOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/admin/login/index");
        }

    }
}
