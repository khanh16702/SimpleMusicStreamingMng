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
    [Route("admin/genre")]
    public class GenreController : Controller
    {
        private readonly IGenreDataProvider _IGenreDataProvider;
        private readonly IWebHostEnvironment _IWebHostEnvironment;
        public IConfiguration Configuration { get; }

        private readonly IStringLocalizer<SharedResource> _sharedLocalizer;
        public GenreController(IGenreDataProvider genreDataProvider, IConfiguration configuration, 
            IStringLocalizer<SharedResource> sharedLocalizer, IWebHostEnvironment iWebHostEnvironment)
        {
            this.Configuration = configuration;
            this._IGenreDataProvider = genreDataProvider;
            this._sharedLocalizer = sharedLocalizer;
            this._IWebHostEnvironment = iWebHostEnvironment;
        }
        [Route(LinkRoute.Index)]
        public IActionResult Index()
        {
            return View();
        }

        [Route("GetGenres")]
        public IActionResult GetGenres(string name)
        {
            IEnumerable<CGenre> genres;
            CResponseMessage response = new CResponseMessage();
            response = _IGenreDataProvider.Search(name);
            string code = response.Code;
            string message = response.Message;
            genres = ((DataSet)response.Data).ToListItem<CGenre>().OrderBy(x => x.Name);
            return Json(new {genres, code, message});
        }

        [Route(LinkRoute.Delete)]
        public IActionResult Delete(int id)
        {
            CResponseMessage cResponseMessage = new CResponseMessage();
            cResponseMessage = _IGenreDataProvider.Delete(id);
            return Json(cResponseMessage);
        }

        [Route(LinkRoute.AddOrUpdate)]
        public IActionResult AddOrUpdate()
        {
            return View();
        }

        [Route(LinkRoute.AddOrUpdate)]
        [HttpPost]
        public IActionResult AddOrUpdate(CGenre model)
        {
            CResponseMessage responseMessage = new CResponseMessage();
            responseMessage = _IGenreDataProvider.Insert(model);
            if (responseMessage.Code == "0")
            {
                return Redirect("/admin/genre/index");
            }
            else
            {
                return Redirect("/admin/genre/addorupdate");
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

            string folderUploads = Path.Combine(_IWebHostEnvironment.WebRootPath, "admin\\assets\\img\\genres");
            string fileName = Guid.NewGuid().ToString() + file.FileName;
            string fullPath = Path.Combine(folderUploads, fileName);
            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                file.CopyTo(stream);
            }
            string filePath = "/admin/assets/img/genres/" + fileName;
            return Json(new
            {
                status = "success",
                filePath
            });
        }
    }
}
