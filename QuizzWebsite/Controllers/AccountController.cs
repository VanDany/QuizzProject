using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizzWebsite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace QuizzWebsite.Controllers
{
    public class AccountController : Controller
    {
        private readonly HttpClient _client;
        private readonly ILogger<AccountController> _logger;
        private readonly string _getUserInDB;

        public AccountController(ILogger<AccountController> logger, IConfiguration _config, HttpClient client)
        {
            _logger = logger;
            _client = client;
            _getUserInDB = _config["QuizzServiceAudiences:getUserInDB"];
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task Login(string returnUrl = "/")
        {
            await HttpContext.ChallengeAsync("Auth0", new AuthenticationProperties() { RedirectUri = returnUrl });
        }

        [Authorize]
        public async Task Logout()
        {
            await HttpContext.SignOutAsync("Auth0", new AuthenticationProperties
            {
                RedirectUri = Url.Action("Index", "Home")
            });
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
        public IActionResult AccessDenied()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> Profile()
        {
            ClaimAccessToken();
            var httpResponse = await _client.GetAsync($"{_getUserInDB}/?username={User.Identity.Name}");
            if (!httpResponse.IsSuccessStatusCode)
            {
                throw new Exception("cannot retrieve user");
            }
            var content = await httpResponse.Content.ReadAsStringAsync();
            var lst = JsonConvert.DeserializeObject<mUserInDB>(content);
            ViewBag.GoodAnswers = lst.GoodAnswersTot;
            ViewBag.WrongAnswers = lst.WrongAnswersTot;
            ViewBag.QuizzTotal = lst.QuizzTot;
            double? ratio = Math.Round(((double)lst.GoodAnswersTot / ((double)lst.WrongAnswersTot+(double)lst.GoodAnswersTot) * 100),2);
            ViewBag.Ratio = ratio;
            return View(new mUserProfile()
            {
                Name = User.Identity.Name,
                EmailAddress = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value,
                ProfileImage = User.Claims.FirstOrDefault(c => c.Type == "picture")?.Value
            });
        }

        [Authorize]
        public IActionResult Claims()
        {
            return View();
        }
        private async void ClaimAccessToken()
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        }
    }
}

