using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DogShow.ImageServer.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace DogShow.ImageServer.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHostingEnvironment _appEnvironment;
        public HomeController(IHostingEnvironment appEnvironment)
        {
            _appEnvironment = appEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddFile(IFormFile uploadedFile,string avatarType)
        {
            if (uploadedFile == null) return RedirectToAction("Index");
            var path = $"/Files/{avatarType}/" + uploadedFile.FileName;
            using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                await uploadedFile.CopyToAsync(fileStream);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> GetPhoto(object photoName, object avatarType)
        {
            if (photoName == null) return RedirectToAction("Error");
            var path = $"/Files/{avatarType}/" + photoName;
            var image = System.IO.File.OpenRead(path);
            return File(image, "image/jpeg");
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
