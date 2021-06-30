using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using QuizzWebsite.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace QuizzWebsite.Controllers
{


    public class HomeController : Controller
    {
        private readonly string _getAll;

        private readonly string _getAllPublic;

        private readonly string _getQuestions;

        private readonly string _getQuestionsCount;

        private readonly string _createScoreInDB;
        private readonly string _getUserInDB;

        private readonly HttpClient _client;

        private readonly ILogger<HomeController> _logger;
        
        public HomeController(ILogger<HomeController> logger, IConfiguration _config, HttpClient client)
        {
            _logger = logger;
            _client = client;
            //Audiences
            _getAll = _config["QuizzServiceAudiences:getAll"];
            _getAllPublic = _config["QuizzServiceAudiences:getAllPublic"];
            _getQuestions = _config["QuizzServiceAudiences:getQuestions"];
            _getQuestionsCount = _config["QuizzServiceAudiences:getQuestionsCount"];
            _createScoreInDB = _config["QuizzServiceAudiences:createScoreInDB"];
            _getUserInDB = _config["QuizzServiceAudiences:getUserInDB"];
        }
        public async Task<IActionResult> Index()
        {
            HttpContext.Session.SetInt32("score", 0);
            HttpContext.Session.SetInt32("wrong-score", 0);

            var httpResponse = await _client.GetAsync(_getAllPublic);
            if (!httpResponse.IsSuccessStatusCode)
            {
                throw new Exception("cannot retrieve quizz names");
            }
            var content = await httpResponse.Content.ReadAsStringAsync();
            var lst = JsonConvert.DeserializeObject<List<mQuizz>>(content);
            return View(lst);
        }
        public async Task<IActionResult> Question(int quizzID, int questionID, int? answer)
        {
            
            var score = HttpContext.Session.GetInt32("score");
            var wrongscore = HttpContext.Session.GetInt32("wrong-score");
            //Questions count
            var httpResponse = await _client.GetAsync($"{_getQuestionsCount}/?QuizzID={quizzID}");
            if ((int)httpResponse.StatusCode == 403)
            {
                return RedirectToAction("AccessDenied", "Home");
            }
            if (!httpResponse.IsSuccessStatusCode)
            {
                throw new Exception("cannot retrieve count");
            }
            var content = await httpResponse.Content.ReadAsStringAsync();
            var count = JsonConvert.DeserializeObject<int>(content);
            
            //Good answers
            if (answer == 1)
            {
                score++;
                HttpContext.Session.SetInt32("score", (int) score);
            }

            //Wrong answers
            if (answer == 0 && questionID != 1)
            {
                wrongscore++;
                HttpContext.Session.SetInt32("wrong-score", (int)wrongscore);
            }

            //If quizz is finished, go to results
            if (questionID > count)
            {
                if (User.Identity.IsAuthenticated)
                {
                    ClaimAccessToken();
                    var httpResponseGet = await _client.GetAsync($"{_getUserInDB}/?username={User.Identity.Name}");
                    if (!httpResponseGet.IsSuccessStatusCode)
                    {
                        throw new Exception("cannot retrieve user");
                    }
                    var contentGet = await httpResponseGet.Content.ReadAsStringAsync();
                    var lst = JsonConvert.DeserializeObject<mUserInDB>(contentGet);
                    var contentPOST = new mUserInDB()
                    {
                        id = lst.id,
                        User_Name = User.Identity.Name,
                        GoodAnswersTot = score,
                        WrongAnswersTot = wrongscore,
                        QuizzTot = 1
                    };
                    var jsonToSend = JsonConvert.SerializeObject(contentPOST);
                    var httpResponsePOST = await _client.PostAsync(_createScoreInDB,
                        new StringContent(jsonToSend, Encoding.Default, "application/json"));
                    if (!httpResponsePOST.IsSuccessStatusCode)
                    {
                        throw new Exception("Cannot add score to db");
                    }
                }
                return RedirectToAction("Result");
            }

            //else --> following question
            var httpResponse2 = await _client.GetAsync($"{_getQuestions}/?QuizzID={quizzID}&QuestionID={questionID}");

            if (!httpResponse2.IsSuccessStatusCode)
            {
                throw new Exception("Cannot retrieve questions");
            }

            var content2 = await httpResponse2.Content.ReadAsStringAsync();
            var question = JsonConvert.DeserializeObject<mQuestion>(content2);
            return View(question);
        }
        public async Task<IActionResult> listeTEST()
        {
            if (User.Identity.IsAuthenticated)
            {
                ClaimAccessToken();
                var httpResponse = await _client.GetAsync(_getAll);
                if ((int)httpResponse.StatusCode == 403)
                {
                    return RedirectToAction("AccessDenied", "Home");
                }
                if (!httpResponse.IsSuccessStatusCode)
                {
                    throw new Exception("cannot retrieve quizz names");
                }
                var content = await httpResponse.Content.ReadAsStringAsync();
                var lst = JsonConvert.DeserializeObject<List<mQuizz>>(content);
                return View(lst);
            }
            return View();
        }
        public IActionResult Result()
        {
            ViewBag.Session = HttpContext.Session.GetInt32("score");

            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult AccessDenied()
        {
            return View();
        }
        //Other methods
        private async void ClaimAccessToken()
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        }
    }
}
