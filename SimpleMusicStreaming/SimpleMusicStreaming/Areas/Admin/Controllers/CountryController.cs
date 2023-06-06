using CoreLib.Config;
using CoreLib.DataTableToObject.Mapping;
using CoreLib.Entities;
using DataServiceLib.Implementations;
using DataServiceLib.Interfaces;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using SimpleMusicStreaming.Areas.Admin.Models;
using System.Data;

namespace SimpleMusicStreaming.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("admin/country")]
    public class CountryController : Controller
    {
        private readonly ICountryDataProvider _ICountryDataProvider;
        public IConfiguration Configuration { get; }
        private readonly IStringLocalizer<SharedResource> _sharedLocalizer;

        public CountryController(ICountryDataProvider iCountryDataProvider, IConfiguration configuration, IStringLocalizer<SharedResource> sharedLocalizer)
        {
            _ICountryDataProvider = iCountryDataProvider;
            Configuration = configuration;
            _sharedLocalizer = sharedLocalizer;
        }

        [Route(LinkRoute.Index)]
        public IActionResult Index()
        {
            return View();
        }

        [Route("GetCountries")]
        public IActionResult GetCountries(string name)
        {
            IEnumerable<CCountry> countriesView;
            CResponseMessage response = new CResponseMessage();
            response = _ICountryDataProvider.Search(name);
            string code = response.Code;
            string message = response.Message;
            countriesView = ((DataSet)response.Data).ToListItem<CCountry>().OrderBy(x => x.Name);
            return Json(new { countriesView, code, message });
        }

        [Route("Delete")]
        public IActionResult Delete(int id)
        {
            CResponseMessage cResponseMessage = new CResponseMessage();
            cResponseMessage = _ICountryDataProvider.Delete(id);
            return Json(cResponseMessage);
        }

        [Route(LinkRoute.AddOrUpdate)]
        public IActionResult AddOrUpdate(int id)
        {
            var countryModel = new CCountry();
            if (id > 0)
            {
                CResponseMessage cResponseMessage = new CResponseMessage();
                cResponseMessage = _ICountryDataProvider.GetDetail(id);
                countryModel = ((DataSet)cResponseMessage.Data).ToListItem<CCountry>().FirstOrDefault();
            }
            return View(countryModel);
        }

        [Route(LinkRoute.AddOrUpdate)]
        [HttpPost]
        public IActionResult AddOrUpdate(CCountry model)
        {
            CResponseMessage responseMessage = new CResponseMessage();
            if (model.Id > 0)
            {
                responseMessage = _ICountryDataProvider.Update(model);
            }
            else
            {
                responseMessage = _ICountryDataProvider.Insert(model);
            }
            if (responseMessage.Code == "0")
            {
                return Redirect("/admin/country/index");
            }
            else
            {
                return Redirect("/admin/country/addorupdate?id=" + model.Id);
            }
        }
    }
}
