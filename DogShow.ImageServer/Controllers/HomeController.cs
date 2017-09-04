using System;
using System.Diagnostics;
using System.IO;
using System.Net;
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
        public async Task<IActionResult> AddFile(FileModel model)
        {
            if (!ModelState.IsValid) return BadRequest("Invalid model");
            Console.WriteLine(model.File.Length);
            var path = $"/Files/{model.Avatar}/" + model.File.FileName;
            using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                await model.File.CopyToAsync(fileStream);
            Console.WriteLine(_appEnvironment.WebRootPath + path);
            return RedirectToAction("Index");
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }

    public class FileModel
    {
        public IFormFile File { get; set; }
        public string Avatar { get; set; }
    }
}
