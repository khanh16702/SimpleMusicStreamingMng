using CoreLib.Config;
using CoreLib.DataTableToObject.Mapping;
using CoreLib.Entities;
using DataServiceLib.Implementations;
using DataServiceLib.Interfaces;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using SimpleMusicStreaming.App_Code;
using SimpleMusicStreaming.Areas.Admin.Models;
using System.Data;
using System.IO;

namespace SimpleMusicStreaming.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("admin/artist")]
    public class ArtistController : Controller
    {
        private readonly IArtistDataProvider _IArtistDataProvider;
        private readonly ICountryDataProvider _ICountryDataProvider;
        private readonly IWebHostEnvironment _IWebHostEnvironment;
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Chuyển đồi thôngtin Mess lỗi => Mess hiển thị.
        /// </summary>
        private readonly IStringLocalizer<SharedResource> _sharedLocalizer;
        public ArtistController(IArtistDataProvider artistDataProvider, ICountryDataProvider countryDataProvider,
           IConfiguration configuration, IStringLocalizer<SharedResource> sharedLocalizer, IWebHostEnvironment iWebHostEnvironment)
        {
            this.Configuration = configuration;
            this._IArtistDataProvider = artistDataProvider;
            this._ICountryDataProvider = countryDataProvider;
            this._sharedLocalizer = sharedLocalizer;
            this._IWebHostEnvironment = iWebHostEnvironment;
        }
        [Route(LinkRoute.Index)]
        public IActionResult Index()
        {
            return View();
        }

        [Route("GetArtists")]
        public async Task<JsonResult> GetArtists(string name, int countryId)
        {
            string prm = "?name=" + name + "&countryId=" + countryId;
            var artistObject = await CCallAPI.SearchTemplateAsync("api/artistapi/GetArtists" + prm, Configuration);
            var jsonCode = JsonConvert.SerializeObject(artistObject);
            return Json(jsonCode);
        }

        public async Task<JsonResult> Delete(int id)
        {
            var response = await CCallAPI.DeleteTemplateAsync("api/artistapi/delete?id=" + id, Configuration);
            return Json(response);
        }

        [Route(LinkRoute.UploadFile)]
        [HttpPost]
        public IActionResult UploadFile(IFormFile file)
        {
            if (file == null)
            {
                return Json(new { status = "error" });
            }

            string folderUploads = Path.Combine(_IWebHostEnvironment.WebRootPath, "admin\\assets\\img\\artists");
            string fileName = Guid.NewGuid().ToString() + file.FileName;
            string fullPath = Path.Combine(folderUploads, fileName);
            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                file.CopyTo(stream);
            }
            string filePath = "/admin/assets/img/artists/" + fileName;
            return Json(new
            {
                status = "success",
                filePath
            });
        }

        [Route("DeleteUnusedFile")]
        public IActionResult DeleteUnusedFile(string path, string defaultImage)
        {
            string deletePath = Path.Combine(_IWebHostEnvironment.WebRootPath, path);
            string defaultImagePath = Path.Combine(_IWebHostEnvironment.WebRootPath, defaultImage);
            if (System.IO.File.Exists(deletePath) && deletePath != defaultImagePath)
            {
                System.IO.File.Delete(deletePath);
            }
            return Json(new { deletePath, defaultImagePath });
        }

        [Route(LinkRoute.AddOrUpdate)]
        [HttpPost]
        public async Task<JsonResult> AddOrUpdate(CArtist model)
        {
            var response = await CCallAPI.PostTemplateAsync(model, APILinkRoute.ArtistAddOrUpdate, Configuration);
            return Json(response);
        }
    }
}
