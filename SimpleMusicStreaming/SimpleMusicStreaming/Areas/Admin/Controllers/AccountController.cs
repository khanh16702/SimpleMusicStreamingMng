using Microsoft.AspNetCore.Mvc;
using CoreLib.Config;
using DataServiceLib.Interfaces;
using Microsoft.Extensions.Localization;
using CoreLib.Entities;
using DataServiceLib.Implementations;
using SimpleMusicStreaming.Areas.Admin.Models;
using System.Data;
using CoreLib.DataTableToObject.Mapping;

namespace SimpleMusicStreaming.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("admin/account")]
    public class AccountController : Controller
    {
        private readonly IAccountDataProvider _IAccountDataProvider;
        private readonly IWebHostEnvironment _IWebHostEnvironment;
        public IConfiguration Configuration { get; }
        private readonly IStringLocalizer<SharedResource> _sharedLocalizer;
        public AccountController(IAccountDataProvider accountDataProvider,
           IConfiguration configuration, IStringLocalizer<SharedResource> sharedLocalizer, IWebHostEnvironment iWebHostEnvironment)
        {
            this.Configuration = configuration;
            this._IAccountDataProvider = accountDataProvider;
            this._sharedLocalizer = sharedLocalizer;
            this._IWebHostEnvironment = iWebHostEnvironment;
        }
        [Route(LinkRoute.Index)]
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [Route("GetAccounts")]
        [HttpGet]
        public IActionResult GetAccounts(string name)
        {
            IEnumerable<CAccount> accounts;
            CResponseMessage response = new CResponseMessage();
            response = _IAccountDataProvider.Search(name);
            string code = response.Code;
            string message = response.Message;
            accounts = ((DataSet)response.Data).ToListItem<CAccount>();
            return Json(new { accounts, code, message });
        }


    }
}
