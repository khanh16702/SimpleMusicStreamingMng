using CoreLib.Config;
using CoreLib.DataTableToObject.Mapping;
using CoreLib.Entities;
using DataServiceLib.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Data;

namespace SimpleMusicStreaming.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("admin/home")]
    public class HomeController : Controller
    {
        private readonly IAlbumDataProvider _IAlbumDataProvider;
        private readonly IArtistDataProvider _IArtistDataProvider;
        private readonly IWebHostEnvironment _IWebHostEnvironment;
        private readonly ITrackDataProvider _ITrackDataProvider;
        private readonly IGenreDataProvider _IGenreDataProvider;
        private readonly ICountryDataProvider _ICountryDataProvider;
        public IConfiguration Configuration { get; }

        private readonly IStringLocalizer<SharedResource> _sharedLocalizer;
        public HomeController(IAlbumDataProvider albumDataProvider, IArtistDataProvider artistDataProvider, IConfiguration configuration,
            IStringLocalizer<SharedResource> sharedLocalizer, IWebHostEnvironment iWebHostEnvironment, ITrackDataProvider iTrackDataProvider, IGenreDataProvider iGenreDataProvider, ICountryDataProvider iCountryDataProvider)
        {
            this.Configuration = configuration;
            this._IAlbumDataProvider = albumDataProvider;
            this._IArtistDataProvider = artistDataProvider;
            this._sharedLocalizer = sharedLocalizer;
            this._IWebHostEnvironment = iWebHostEnvironment;
            this._ITrackDataProvider = iTrackDataProvider;
            this._IGenreDataProvider = iGenreDataProvider;
            this._ICountryDataProvider = iCountryDataProvider;
        }
        [Route(LinkRoute.Index)]
        public IActionResult Index()
        {
            int albums;
            int artists;
            int tracks;
            int genres;
            int countries;
            CResponseMessage response = _IAlbumDataProvider.Search("");
            albums = ((DataSet)response.Data).ToListItem<CAlbum>().Count();
            ViewBag.albumsCount = albums;

            response = _IArtistDataProvider.Search("");
            artists = ((DataSet)response.Data).ToListItem<CArtist>().Count();
            ViewBag.artistsCount = artists;

            response = _ITrackDataProvider.Search("");
            tracks = ((DataSet)response.Data).ToListItem<CTrack>().Count();
            ViewBag.tracksCount = tracks;

            response = _IGenreDataProvider.Search("");
            genres = ((DataSet)response.Data).ToListItem<CGenre>().Count();
            ViewBag.genresCount = genres;

            response = _IGenreDataProvider.Search("");
            genres = ((DataSet)response.Data).ToListItem<CGenre>().Count();
            ViewBag.genresCount = genres;

            response = _ICountryDataProvider.Search("");
            countries = ((DataSet)response.Data).ToListItem<CCountry>().Count();
            ViewBag.countriesCount = countries;
            return View();
        }
    }
}
