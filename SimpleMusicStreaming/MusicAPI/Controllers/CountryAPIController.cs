using CoreLib.Config;
using CoreLib.DataTableToObject.Mapping;
using CoreLib.Entities;
using DataServiceLib.Interfaces;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json.Linq;
using SimpleMusicStreaming;
using System.Data;

namespace MusicAPI.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class CountryAPIController : ControllerBase
    {
        private readonly ICountryDataProvider _ICountryDataProvider;
        public IConfiguration Configuration { get; }
        private readonly IStringLocalizer<SharedResource> _sharedLocalizer;

        public CountryAPIController(ICountryDataProvider iCountryDataProvider, IConfiguration configuration, IStringLocalizer<SharedResource> sharedLocalizer)
        {
            _ICountryDataProvider = iCountryDataProvider;
            Configuration = configuration;
            _sharedLocalizer = sharedLocalizer;
        }

        [HttpGet]
        public JsonResult GetAllCountries()
        {
            CResponseMessage countryResponse = new CResponseMessage();
            countryResponse = _ICountryDataProvider.Search("");
            IEnumerable<CCountry> countries;
            countries = ((DataSet)countryResponse.Data).ToListItem<CCountry>();
            List<CCountry> lst = new List<CCountry>();
            foreach (var country in countries)
            {
                if (country.IsActive)
                {
                    lst.Add(country);
                }
            }
            return new JsonResult(lst.OrderBy(x => x.Name));
        }
    }
}
