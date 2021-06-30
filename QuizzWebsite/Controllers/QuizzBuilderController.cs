using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using QuizzWebsite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using System.Net.Http.Headers;

namespace QuizzWebsite.Controllers
{
    public class QuizzBuilderController : Controller
    {
        private readonly string _postQuizz;
        private readonly string _postQuestions;
        private readonly string _deleteQuizz;
        private readonly string _getQuizzByID;
        private readonly string _putQuestions;
        private readonly string _putQuizz;
        private readonly ILogger<QuizzBuilderController> _logger;
        private readonly HttpClient _client;
        public QuizzBuilderController(ILogger<QuizzBuilderController> logger, IConfiguration _config, HttpClient client)
        {
            _logger = logger;
            _client = client;
            //Audiences
            _postQuizz = _config["QuizzServiceAudiences:postQuizz"];
            _postQuestions = _config["QuizzServiceAudiences:postQuestions"];
            _deleteQuizz = _config["QuizzServiceAudiences:deleteQuizz"];
            _getQuizzByID = _config["QuizzServiceAudiences:getQuizzByID"];
            _putQuestions = _config["QuizzServiceAudiences:putQuestions"];
            _putQuizz = _config["QuizzServiceAudiences:putQuizz"];
        }
        public IActionResult PostQuizz()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PostQuizz(
            [Bind("QuizzID, CreationDate, Title, Category, Difficulty, AverageScore, QuestionsCount")]
            mQuizz mQuizz)
        {
            if (ModelState.IsValid && User.Identity.IsAuthenticated)
            {
                ClaimAccessToken();
                var content = JsonConvert.SerializeObject(mQuizz);
                var httpResponse = await _client.PostAsync(_postQuizz,
                    new StringContent(content, Encoding.Default, "application/json"));
                if ((int)httpResponse.StatusCode == 403)
                {
                    return RedirectToAction("AccessDenied", "Home");
                }
                if (!httpResponse.IsSuccessStatusCode)
                {
                    throw new Exception("Cannot add a quizz");
                }

                mQuizz quizzID =
                    JsonConvert.DeserializeObject<mQuizz>(await httpResponse.Content.ReadAsStringAsync());
                return RedirectToAction("PostQuestions", new {QuizzID = quizzID.QuizzID});

                //return RedirectToAction("Index", "Home");
            }
            return View(mQuizz);
        }

        public async Task<IActionResult> EditQuizz(int quizzID)
        {
            var httpReponse = await _client.GetAsync($"{_getQuizzByID}/?quizzID={quizzID}");
            if (!httpReponse.IsSuccessStatusCode)
            {
                throw new Exception("Cannot retrieve the quizz with the ID n° " + quizzID + ".");
            }

            var content = await httpReponse.Content.ReadAsStringAsync();
            var tasks = JsonConvert.DeserializeObject<mQuizz>(content);
            return View(tasks);
        }
        

        [HttpPost]
        public async Task<IActionResult> EditQuizz(mQuizz m)
        {
            if (ModelState.IsValid && User.Identity.IsAuthenticated)
            {
                ClaimAccessToken();
                var content = JsonConvert.SerializeObject(m);
                var httpResponse = await _client.PutAsync(_putQuizz,
                    new StringContent(content, Encoding.Default, "application/json"));
                if (!httpResponse.IsSuccessStatusCode)
                {
                    throw new Exception("Cannot add a quizz");
                }

                return RedirectToAction("Index", "Home");
            }

            return null;
        }

        public IActionResult PostQuestions(int QuizzID)
        {
            ViewBag.Message = QuizzID;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PostQuestions(List<mQuestion> mQuestion)
        {
            if (ModelState.IsValid && User.Identity.IsAuthenticated)
            {
                ClaimAccessToken();
                for (int i = 0; i < mQuestion.Count; i++)
                {
                    //This IF prevent bug if user desactivate JS
                    if (mQuestion[i].Question != null)
                    {
                        var content = JsonConvert.SerializeObject(mQuestion.ElementAt(i));
                        var httpResponse = await _client.PostAsync(_postQuestions,
                            new StringContent(content, Encoding.Default, "application/json"));
                        if (!httpResponse.IsSuccessStatusCode)
                        {
                            throw new Exception("Cannot add a quizz");
                        }
                    }
                }
                return RedirectToAction("Index", "Home");
            }
            return View(mQuestion);
        }
        [HttpGet]
        public async Task<mQuizz> QuizzByID(int quizzID)
        {
            var httpReponse = await _client.GetAsync($"{_getQuizzByID}/?quizzID={quizzID}");
            if (!httpReponse.IsSuccessStatusCode)
            {
                throw new Exception("Cannot retrieve the quizz with the ID n° " + quizzID + ".");
            }

            var content = await httpReponse.Content.ReadAsStringAsync();
            var tasks = JsonConvert.DeserializeObject<mQuizz>(content);
            return tasks;
        }

        public async Task<IActionResult> DeleteQuizz(int quizzID)
        {
            var httpResponse = await _client.DeleteAsync($"{_deleteQuizz}/?quizzID={quizzID}");
            if (!httpResponse.IsSuccessStatusCode)
            {
                throw new Exception("Cannot delete the quizz");
            }

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> UpdateQuestion(List<mQuestion> mQuestion)
        {
            if (ModelState.IsValid)
            {
                for (int i = 0; i < mQuestion.Count; i++)
                {
                    //This IF prevent bug if user desactivate JS
                    if (mQuestion[i].Question != null)
                    {
                        var content = JsonConvert.SerializeObject(mQuestion.ElementAt(i));
                        var httpResponse = await _client.PutAsync(_putQuestions,
                            new StringContent(content, Encoding.Default, "application/json"));
                        if (!httpResponse.IsSuccessStatusCode)
                        {
                            throw new Exception("Cannot add a quizz");
                        }
                        var createdTask =
                            JsonConvert.DeserializeObject<mQuestion>(await httpResponse.Content.ReadAsStringAsync());
                    }
                }
                return RedirectToAction("Index", "Home");
            }
            //return View(mQuestion);
            return null;
        }
        //Other methods
        private async void ClaimAccessToken()
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        }
    }
}
