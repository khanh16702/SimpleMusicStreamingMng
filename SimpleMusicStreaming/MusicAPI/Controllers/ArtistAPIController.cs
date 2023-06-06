using CoreLib.Config;
using CoreLib.DataTableToObject.Mapping;
using CoreLib.Entities;
using DataServiceLib.Interfaces;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using MusicAPI.Models;
using Newtonsoft.Json.Linq;
using SimpleMusicStreaming;
using System.Data;

namespace MusicAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArtistAPIController : ControllerBase
    {
        private readonly IArtistDataProvider _IArtistDataProvider;
        private readonly ICountryDataProvider _ICountryDataProvider;
        private readonly IWebHostEnvironment _IWebHostEnvironment;
        public IConfiguration Configuration { get; }

        private readonly IStringLocalizer<SharedResource> _sharedLocalizer;
        public ArtistAPIController(IArtistDataProvider artistDataProvider, ICountryDataProvider countryDataProvider,
           IConfiguration configuration, IStringLocalizer<SharedResource> sharedLocalizer, IWebHostEnvironment iWebHostEnvironment)
        {
            this.Configuration = configuration;
            this._IArtistDataProvider = artistDataProvider;
            this._ICountryDataProvider = countryDataProvider;
            this._sharedLocalizer = sharedLocalizer;
            this._IWebHostEnvironment = iWebHostEnvironment;
        }

        [HttpGet]
        [Route("GetArtists")]
        public JsonResult GetArtists(string? name, int? countryId)
        {
            IEnumerable<CArtist> artists;
            CResponseMessage response = new CResponseMessage();
            if (name == "\"\"")
            {
                response = _IArtistDataProvider.Search("");
            }
            else
            {
                response = _IArtistDataProvider.Search(name);
            }
            string code = response.Code;
            string message = response.Message;
            List<CArtistViewModel> artistsView = new List<CArtistViewModel>();
            if (response.Data != null)
            {
                var country = "";
                CResponseMessage searchCountryMes = new CResponseMessage();
                artists = ((DataSet)response.Data).ToListItem<CArtist>();
                if (countryId > 0)
                {
                    artists = artists.Where(x => x.CountryId == countryId);
                    searchCountryMes = _ICountryDataProvider.GetDetail((int)countryId);
                    country = ((DataSet)searchCountryMes.Data).ToListItem<CCountry>().FirstOrDefault().Name;
                }
                artists = artists.OrderBy(x => x.Name);

                foreach (var artist in artists)
                {
                    var query = new CArtistViewModel()
                    {
                        Id = artist.Id,
                        Name = artist.Name,
                        Image = artist.Image,
                        IsActive = artist.IsActive
                    };

                    if (country != "")
                    {
                        query.CountryName = country;
                    }
                    else
                    {
                        CResponseMessage countryResponse = new CResponseMessage();
                        countryResponse = _ICountryDataProvider.Search("");
                        IEnumerable<CCountry> countries;
                        countries = ((DataSet)countryResponse.Data).ToListItem<CCountry>();

                        var countryCur = countries.Where(x => x.Id == artist.CountryId).FirstOrDefault();
                        string countryName = "";
                        if (countryCur != null)
                        {
                            countryName = countryCur.Name;
                        }
                        query.CountryName = countryName;
                    }
                    artistsView.Add(query);
                    if (artistsView.Count() == 100)
                    {
                        break;
                    }
                }
            }
            return new JsonResult(new { artistsView, code, message });
        }

        [HttpPost]
        [Route("AddOrUpdate")]
        public JsonResult AddOrUpdate(CArtist model)
        {
            CResponseMessage responseMessage = new CResponseMessage();
            if (model.Id > 0)
            {
                responseMessage = _IArtistDataProvider.Update(model);
            }
            else
            {
                responseMessage = _IArtistDataProvider.Insert(model);
            }

            return new JsonResult(responseMessage);
            // 0: success
        }


        [HttpDelete]
        [Route("Delete")]
        public JsonResult Delete(int id)
        {
            CResponseMessage cResponseMessage = new CResponseMessage();
            cResponseMessage = _IArtistDataProvider.Delete(id);
            return new JsonResult(cResponseMessage);
        }

        [HttpGet]
        [Route("GetArtistDetail")]
        public CArtistViewModel GetArtistDetail(int id)
        {
            var artistViewModel = new CArtistViewModel();
            if (id > 0)
            {
                var artistModel = new CArtist();
                CResponseMessage cResponseMessage = new CResponseMessage();
                cResponseMessage = _IArtistDataProvider.GetDetail(id);
                artistModel = ((DataSet)cResponseMessage.Data).ToListItem<CArtist>().FirstOrDefault();

                CResponseMessage responeCountry = new CResponseMessage();
                responeCountry = _ICountryDataProvider.GetDetail(artistModel.CountryId);
                string countryName = ((DataSet)responeCountry.Data).ToListItem<CCountry>().FirstOrDefault().Name;

                artistViewModel.CountryName = countryName;
                artistViewModel.Name = artistModel.Name;
                artistViewModel.CreatedDate = artistModel.CreatedDate;
                artistViewModel.Image = artistModel.Image;
                artistViewModel.IsActive = artistModel.IsActive;
                artistViewModel.Id = artistModel.Id;
            }
            return artistViewModel;
        }

        [HttpGet]
        [Route("GetDefaultImage")]
        public string GetDefaultImage()
        {
            return "/admin/assets/img/artist-.jpg";
        }

    }
}
