using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrtSurvey.Models;
using OrtSurvey.Services.Services;
using System.Diagnostics;

namespace OrtSurvey.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly EncuestaService _encuestaService;

        public HomeController(ILogger<HomeController> logger, EncuestaService encuestaService)
        {
            _logger = logger;
            _encuestaService = encuestaService;
        }

        public IActionResult Index()
        {
            ViewBag.EncuestasPublicas = _encuestaService.GetPublicas(9);
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Registro()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [Authorize]
        public IActionResult CrearEncuesta()
        {
            return View();
        }

        [Authorize]
        public IActionResult MisEncuestas()
        {
            return View();
        }

        [Authorize]
        public IActionResult Metricas(int? idEncuesta)
        {
            ViewBag.IdEncuesta = idEncuesta;
            return View();
        }

        public IActionResult ResponderEncuesta(string id)
        {
            ViewBag.EncuestaCodigo = id;
            return View();
        }
    }
}
