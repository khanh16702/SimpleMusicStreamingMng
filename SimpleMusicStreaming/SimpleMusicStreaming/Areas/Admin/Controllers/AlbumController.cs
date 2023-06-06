using CoreLib.Config;
using CoreLib.DataTableToObject.Mapping;
using CoreLib.Entities;
using DataServiceLib.Implementations;
using DataServiceLib.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using SimpleMusicStreaming.Areas.Admin.Models;
using System.Data;

namespace SimpleMusicStreaming.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("admin/album")]
    public class AlbumController : Controller
    {
        private readonly IAlbumDataProvider _IAlbumDataProvider;
        private readonly IArtistDataProvider _IArtistDataProvider;
        private readonly IWebHostEnvironment _IWebHostEnvironment;
        public IConfiguration Configuration { get; }

        private readonly IStringLocalizer<SharedResource> _sharedLocalizer;
        public AlbumController(IAlbumDataProvider albumDataProvider, IArtistDataProvider artistDataProvider, IConfiguration configuration,
            IStringLocalizer<SharedResource> sharedLocalizer, IWebHostEnvironment iWebHostEnvironment)
        {
            this.Configuration = configuration;
            this._IAlbumDataProvider = albumDataProvider;
            this._IArtistDataProvider = artistDataProvider;
            this._sharedLocalizer = sharedLocalizer;
            this._IWebHostEnvironment = iWebHostEnvironment;
        }

        [Route(LinkRoute.Index)]
        public IActionResult Index()
        {
            return View();
        }

        [Route("GetAlbums")]
        public IActionResult GetAlbums(string name, int artistId)
        {
            IEnumerable<CAlbum> albums;
            CResponseMessage response = new CResponseMessage();
            response = _IAlbumDataProvider.Search(name);
            string code = response.Code;
            string message = response.Message;
            List<CAlbumViewModel> albumsView = new List<CAlbumViewModel>();
            if (response.Data != null)
            {
                var artist = "";
                CResponseMessage searchCountryMes = new CResponseMessage();
                albums = ((DataSet)response.Data).ToListItem<CAlbum>();
                if (artistId > 0)
                {
                    albums = albums.Where(x => x.ArtistId == artistId);
                    searchCountryMes = _IArtistDataProvider.GetDetail(artistId);
                    artist = ((DataSet)searchCountryMes.Data).ToListItem<CArtist>().FirstOrDefault().Name;
                }
                albums = albums.OrderBy(x => x.Name);

                foreach (var album in albums)
                {
                    var query = new CAlbumViewModel()
                    {
                        Id = album.Id,
                        Name = album.Name,
                        Image = album.Image,
                        CreatedDate = album.CreatedDate,
                        IsActive = album.IsActive
                    };

                    if (artist != "")
                    {
                        query.ArtistName = artist;
                    }
                    else
                    {
                        CResponseMessage artistResponse = new CResponseMessage();
                        artistResponse = _IArtistDataProvider.Search("");
                        IEnumerable<CArtist> artists;
                        artists = ((DataSet)artistResponse.Data).ToListItem<CArtist>();

                        var artistCur = artists.Where(x => x.Id == album.ArtistId).FirstOrDefault();
                        string artistName = "";
                        if (artistCur != null)
                        {
                            artistName = artistCur.Name;
                        }
                        query.ArtistName = artistName;
                    }
                    albumsView.Add(query);
                    if (albumsView.Count() == 100)
                    {
                        break;
                    }
                }
            }
            return Json(new { albumsView, code, message });
        }

        [Route("GetAllArtists")]
        public IActionResult GetAllArtists()
        {
            CResponseMessage artistResponse = new CResponseMessage();
            artistResponse = _IArtistDataProvider.Search("");
            IEnumerable<CArtist> artists;
            artists = ((DataSet)artistResponse.Data).ToListItem<CArtist>();
            List<CArtist> lst = new List<CArtist>();
            foreach (var artist in artists)
            {
                if (artist.IsActive)
                {
                    lst.Add(artist);
                }
            }
            return Json(lst.OrderBy(x => x.Name));
        }

        [Route(LinkRoute.AddOrUpdate)]
        public IActionResult AddOrUpdate()
        {
            return View();
        }

        [Route(LinkRoute.AddOrUpdate)]
        [HttpPost]
        public IActionResult AddOrUpdate(CAlbum model)
        {
            CResponseMessage responseMessage = new CResponseMessage();
            responseMessage = _IAlbumDataProvider.Insert(model);
            if (responseMessage.Code == "0")
            {
                return Redirect("/admin/album/index");
            }
            else
            {
                return Redirect("/admin/album/addorupdate");
            }
        }

        [Route(LinkRoute.UploadFile)]
        [HttpPost]
        public IActionResult UploadFile(IFormFile file)
        {
            if (file == null)
            {
                return Json(new { status = "error" });
            }

            string folderUploads = Path.Combine(_IWebHostEnvironment.WebRootPath, "admin\\assets\\img\\albums");
            string fileName = Guid.NewGuid().ToString() + file.FileName;
            string fullPath = Path.Combine(folderUploads, fileName);
            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                file.CopyTo(stream);
            }
            string filePath = "/admin/assets/img/albums/" + fileName;
            return Json(new
            {
                status = "success",
                filePath
            });
        }

    }
}
