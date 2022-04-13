using GoogleRecaptcha.Web.Models;
using GoogleRecaptcha.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Diagnostics;

namespace GoogleRecaptcha.Web.Controllers
{

    //[ValidateReCaptcha]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly GoogleRecaptchaService _googleRecaptchaService;

        public HomeController(
            ILogger<HomeController> logger,
            GoogleRecaptchaService googleRecaptchaService
            )
        {
            _logger = logger;
            _googleRecaptchaService = googleRecaptchaService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View(new LoginModel());
        }


        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            var response = Request.Form["g-Recaptcha-Response"];
            if (ModelState.IsValid)
            {
                if (await _googleRecaptchaService.VirifyTokenAsync(model.Token))
                {
                    return RedirectToAction("Index");
                }
                
            }
            _logger.Log( LogLevel.Warning, "Recaptcha failed!");

            return RedirectToAction("Recaptcha", new LoginModel());
        }


        [HttpPost]
        public async Task<IActionResult> LoginWithReCaptcha(LoginModel model)
        {
            //g-recaptcha-response
            var response = Request.Form["g-Recaptcha-Response"];
            if (ModelState.IsValid)
            {
                if (await _googleRecaptchaService.VirifyTokenAsync(model.Token, true))
                {
                    return RedirectToAction("Index");
                }
            }
            _logger.Log(LogLevel.Warning, "Recaptcha failed!");
            return RedirectToAction("Login");
        }
        public IActionResult Recaptcha()
        {
            return View(new LoginModel());
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
    }
}