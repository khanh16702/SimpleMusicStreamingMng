using CoreLib.Config;
using CoreLib.DataTableToObject.Mapping;
using CoreLib.Entities;
using DataServiceLib.Implementations;
using DataServiceLib.Interfaces;
using MessagePack.Formatters;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using NAudio.Wave;
using SimpleMusicStreaming.Areas.Admin.Models;
using System.Data;
using CArtistOfTrack = CoreLib.Entities.CArtistOfTrack;

namespace SimpleMusicStreaming.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("admin/track")]
    public class TrackController : Controller
    {
        private readonly ITrackDataProvider _ITrackDataProvider;
        private readonly IWebHostEnvironment _IWebHostEnvironment;
        private readonly IAlbumDataProvider _IAlbumDataProvider;
        private readonly IArtistDataProvider _IArtistDataProvider;
        private readonly IArtistOfTrackDataProvider _IArtistOfTrackDataProvider;
        private readonly IGenreOfTrackDataProvider _IGenreOfTrackDataProvider;
        private readonly IGenreDataProvider _IGenreDataProvider;
        public IConfiguration Configuration { get; }
        private readonly IStringLocalizer<SharedResource> _sharedLocalizer;
        public TrackController(ITrackDataProvider trackDataProvider, IAlbumDataProvider albumDataProvider,
           IConfiguration configuration, IStringLocalizer<SharedResource> sharedLocalizer, IWebHostEnvironment iWebHostEnvironment,
           IArtistDataProvider artistDataProvider, IArtistOfTrackDataProvider iArtistOfTrackDataProvider, IGenreOfTrackDataProvider iGenreOfTrackDataProvider, 
           IGenreDataProvider iGenreDataProvider)
        {
            this.Configuration = configuration;
            this._ITrackDataProvider = trackDataProvider;
            this._sharedLocalizer = sharedLocalizer;
            this._IWebHostEnvironment = iWebHostEnvironment;
            this._IAlbumDataProvider = albumDataProvider;
            this._IArtistDataProvider = artistDataProvider;
            this._IArtistOfTrackDataProvider = iArtistOfTrackDataProvider;
            this._IGenreOfTrackDataProvider = iGenreOfTrackDataProvider;
            this._IGenreDataProvider = iGenreDataProvider;
        }

        [Route(LinkRoute.Index)]
        public IActionResult Index()
        {
            return View();
        }

        [Route("GetAllArtists")]
        public IActionResult GetAllArtists(int authorOfAlbumId)
        {
            CResponseMessage artistResponse = new CResponseMessage();
            artistResponse = _IArtistDataProvider.Search("");
            IEnumerable<CArtist> artists;
            artists = ((DataSet)artistResponse.Data).ToListItem<CArtist>().Where(x => x.IsActive == true && x.Id != authorOfAlbumId);
            return Json(artists.OrderBy(x => x.Name).ToList());
        }

        [Route("GetAllGenres")]
        public IActionResult GetAllGenres()
        {
            CResponseMessage genreResponse = new CResponseMessage();
            genreResponse = _IGenreDataProvider.Search("");
            IEnumerable<CGenre> genres;
            genres = ((DataSet)genreResponse.Data).ToListItem<CGenre>().Where(x => x.IsActive == true);
            return Json(genres.OrderBy(x => x.Name).ToList());
        }

        [Route("GetAlbums")]
        public IActionResult GetAlbums(int artistId)
        {
            CResponseMessage cResponseMessage = new CResponseMessage();
            cResponseMessage = _IAlbumDataProvider.Search("");
            IEnumerable<CAlbum> albums = ((DataSet)cResponseMessage.Data).ToListItem<CAlbum>().Where(x => x.ArtistId == artistId).ToList();
            return Json(albums.OrderBy(x => x.Name));
        }

        [Route("GetTracks")]
        public IActionResult GetTracks(string name, int artistId, int albumId)
        {
            IEnumerable<CTrack> tracks;
            CResponseMessage response = new CResponseMessage();
            response = _ITrackDataProvider.Search(name);
            string code = response.Code;
            string message = response.Message;
            List<CTrackViewModel> tracksView = new List<CTrackViewModel>();
            if (response.Data != null)
            {
                var album = "";
                var artist = "";
                CResponseMessage searchAlbumMes = new CResponseMessage();
                tracks = ((DataSet)response.Data).ToListItem<CTrack>();
                if (albumId > 0)
                {
                    tracks = tracks.Where(x => x.AlbumId == albumId);
                    searchAlbumMes = _IAlbumDataProvider.GetDetail(albumId);
                    album = ((DataSet)searchAlbumMes.Data).ToListItem<CAlbum>().FirstOrDefault().Name;
                }
                if (artistId > 0)
                {
                    CResponseMessage responseArtistAndTrack = new CResponseMessage();
                    responseArtistAndTrack = _IArtistOfTrackDataProvider.GetArtistOfTrack();
                    IEnumerable<CArtistOfTrack> artistOfTracks = ((DataSet)responseArtistAndTrack.Data).ToListItem<CArtistOfTrack>()
                        .Where(x => x.ArtistId == artistId)
                        .ToList();

                    string whereString = "";
                    foreach (var artistOfTrack in artistOfTracks)
                    {
                        whereString += artistOfTrack.TrackId + ",";
                    }
                    tracks = tracks.Where(x => whereString.Contains(x.Id.ToString()));
                }
                tracks = tracks.OrderBy(x => x.Name);

                foreach (var track in tracks)
                {
                    var query = new CTrackViewModel()
                    {
                        Id = track.Id,
                        Name = track.Name,
                        Image = track.Image,
                        IsActive = track.IsActive,
                        Duration = track.Duration,
                        Media = track.Media
                    };

                    if (album != "")
                    {
                        query.AlbumName = album;
                    }
                    else
                    {
                        CResponseMessage albumResponse = new CResponseMessage();
                        albumResponse = _IAlbumDataProvider.Search("");
                        IEnumerable<CAlbum> albums;
                        albums = ((DataSet)albumResponse.Data).ToListItem<CAlbum>();

                        var albumCur = albums.Where(x => x.Id == track.AlbumId).FirstOrDefault();
                        string albumName = "";
                        if (albumCur != null)
                        {
                            albumName = albumCur.Name;
                        }
                        query.AlbumName = albumName;
                    }

                    string artistsList = "";
                    CResponseMessage findArtistResponse = new CResponseMessage();
                    findArtistResponse = _IArtistOfTrackDataProvider.GetArtistOfTrack();
                    IEnumerable<CArtistOfTrack> artistsId = ((DataSet)findArtistResponse.Data)
                        .ToListItem<CArtistOfTrack>()
                        .Where(x => x.TrackId == track.Id)
                        .ToList();
                    foreach (var a in artistsId)
                    {
                        string currentArtistName = ((DataSet)_IArtistDataProvider.GetDetail(a.ArtistId).Data)
                            .ToListItem<CArtist>()
                            .FirstOrDefault().Name;
                        artistsList += currentArtistName + ", ";
                    }
                    artistsList = artistsList.Substring(0, artistsList.Length - 2);
                    query.ArtistName = artistsList;


                    string genresList = "";
                    CResponseMessage findGenreResponse = new CResponseMessage();
                    findGenreResponse = _IGenreOfTrackDataProvider.GetGenreOfTrack();
                    IEnumerable<CGenreOfTrack> genresId = ((DataSet)findGenreResponse.Data)
                        .ToListItem<CGenreOfTrack>()
                        .Where(x => x.TrackId == track.Id)
                        .ToList();
                    foreach (var a in genresId)
                    {
                        string currentGenreName = ((DataSet)_IGenreDataProvider.GetDetail(a.GenreId).Data)
                            .ToListItem<CGenre>()
                            .FirstOrDefault().Name;
                        genresList += currentGenreName + ", ";
                    }
                    genresList = genresList.Substring(0, genresList.Length - 2);
                    query.GenreName = genresList;


                    tracksView.Add(query);

                    if (tracksView.Count() == 1000)
                    {
                        break;
                    }
                }
            }

            return Json(new { tracksView, code, message });
        }

        [Route(LinkRoute.AddOrUpdate)]
        public IActionResult AddOrUpdate(int id)
        {
            var trackViewModel = new CTrackViewModel();
            if (id > 0)
            {
                var trackModel = new CTrack();
                CResponseMessage cResponseMessage = new CResponseMessage();
                cResponseMessage = _ITrackDataProvider.GetDetail(id);
                trackModel = ((DataSet)cResponseMessage.Data).ToListItem<CTrack>().FirstOrDefault();

                CResponseMessage responseAlbum = new CResponseMessage();
                responseAlbum = _IAlbumDataProvider.GetDetail(trackModel.AlbumId);
                var albumObj = ((DataSet)responseAlbum.Data).ToListItem<CAlbum>().FirstOrDefault();
                var albumName = albumObj.Name;

                string artistsList = "";
                CResponseMessage findArtistResponse = new CResponseMessage();
                findArtistResponse = _IArtistOfTrackDataProvider.GetArtistOfTrack();
                IEnumerable<CArtistOfTrack> artistsId = ((DataSet)findArtistResponse.Data)
                    .ToListItem<CArtistOfTrack>()
                    .Where(x => x.TrackId == id)
                    .ToList();
                foreach (var a in artistsId)
                {
                    string currentArtistName = ((DataSet)_IArtistDataProvider.GetDetail(a.ArtistId).Data)
                        .ToListItem<CArtist>()
                        .FirstOrDefault().Name;
                    artistsList += currentArtistName + ", ";
                }
                artistsList = artistsList.Substring(0, artistsList.Length - 2);

                trackViewModel.ArtistName = artistsList;
                trackViewModel.AlbumName = albumName;
                trackViewModel.Name = trackModel.Name;
                trackViewModel.Image = trackModel.Image;
                trackViewModel.Duration = trackModel.Duration;
                trackViewModel.CreatedDate = trackModel.CreatedDate;
                trackViewModel.Media = trackModel.Media;
                trackViewModel.IsActive = trackModel.IsActive;
                trackViewModel.Id = id;
            }
            return View(trackViewModel);
        }

        [Route(LinkRoute.AddOrUpdate)]
        [HttpPost]
        public IActionResult AddOrUpdate(CTrack model, List<int> artistsIdList, List<int> genresIdList, int authorOfAlbumId,
            IFormFile audioUpload)
        {
            CResponseMessage cResponse = new CResponseMessage();
            if (model.Id > 0)
            {
                cResponse = _ITrackDataProvider.Update(model);
            }
            else
            {
                if (model.AlbumId == 0 || model.Name == "" || genresIdList.Count() == 0)
                {
                    TempData["Error"] = "Lack of neccessary information";
                    return Redirect("/admin/track/addorupdate?id=0");
                }

                if (audioUpload == null)
                {
                    TempData["Error"] = "No file found";
                    return Redirect("/admin/track/addorupdate?id=0");
                }

                string folderUploads = Path.Combine(_IWebHostEnvironment.WebRootPath, "admin\\assets\\audio");
                string fileName = Guid.NewGuid().ToString() + audioUpload.FileName;
                string fullPath = Path.Combine(folderUploads, fileName);
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    audioUpload.CopyTo(stream);
                }
                string filePath = "/admin/assets/audio/" + fileName;
                model.Media = filePath;

                Mp3FileReader reader = new Mp3FileReader(fullPath);
                TimeSpan songDuration = reader.TotalTime;
                double duration = songDuration.TotalSeconds;
                model.Duration = duration;

                string artistsIdListToString = "";
                foreach (var i in artistsIdList)
                {
                    if (i == 0)
                    {
                        artistsIdListToString = "";
                        break;
                    }
                    artistsIdListToString += i.ToString() + ",";
                }
                if (artistsIdList.Count() > 0) 
                {
                    artistsIdListToString = artistsIdListToString.Substring(0, artistsIdListToString.Length - 1);
                }

                artistsIdListToString = authorOfAlbumId.ToString() + "," + artistsIdListToString;

                string genresIdListToString = "";

                foreach (var i in genresIdList)
                {
                    genresIdListToString += i.ToString() + ",";
                }

                cResponse = _ITrackDataProvider.Insert(model, artistsIdListToString, genresIdListToString);
            }
            if (cResponse.Code == "0")
            {
                return Redirect("/admin/track/index");
            }
            else
            {
                return Redirect("/admin/track/addorupdate?id=" + model.Id);
            }
        }

        [Route(LinkRoute.Delete)]
        public IActionResult Delete(int id)
        {
            CResponseMessage cResponseMessage = new CResponseMessage();
            cResponseMessage = _ITrackDataProvider.Delete(id);
            return Json(cResponseMessage);
        }

        [Route(LinkRoute.UploadFile)]
        [HttpPost]
        public IActionResult UploadFile(IFormFile file)
        {
            if (file == null)
            {
                return Json(new { status = "error" });
            }

            string folderUploads = Path.Combine(_IWebHostEnvironment.WebRootPath, "admin\\assets\\img\\tracks");
            string fileName = Guid.NewGuid().ToString() + file.FileName;
            string fullPath = Path.Combine(folderUploads, fileName);
            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                file.CopyTo(stream);
            }
            string filePath = "/admin/assets/img/tracks/" + fileName;
            return Json(new
            {
                status = "success",
                filePath
            });
        }
    }
}
