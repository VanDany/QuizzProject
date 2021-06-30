using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using QuizzWebsite.Models;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace QuizzWebsite.Controllers
{
    public class UsersController : Controller
    {
        //for Management Auth0 API
        private string _domain;
        private string _clientId;
        private string _clientSecret;
        private string _audience;
        //myAPI
        private readonly string _getUsers;
        private readonly string _createUsers;
        private readonly string _deleteUsers;
        private readonly string _createUserInDB;
        private readonly string _assignRole;
        private readonly HttpClient _client;

        private readonly ILogger<UsersController> _logger;

        public UsersController(ILogger<UsersController> logger, IConfiguration _config, HttpClient client)
        {
            _logger = logger;
            _client = client;
            _domain = _config["Auth0:Domain"];
            _clientId = _config["Auth0:ClientId"];
            _clientSecret = _config["Auth0:ClientSecret"];
            _audience = _config["Auth0:Audience"];

            _getUsers = _config["QuizzServiceAudiences:getUsers"];
            _createUsers = _config["QuizzServiceAudiences:createUsers"];
            _deleteUsers = _config["QuizzServiceAudiences:deleteUsers"];
            _createUserInDB = _config["QuizzServiceAudiences:createUserInDB"];
            _assignRole = _config["QuizzServiceAudiences:assignRole"];

        }

        public async Task<IActionResult> UsersList()
        {
            if (User.Identity.IsAuthenticated)
            {
                ClaimAccessToken();
                var httpResponse = await _client.GetAsync(_getUsers);
                if ((int)httpResponse.StatusCode == 403)
                {
                    return RedirectToAction("AccessDenied", "Home");
                }
                if (!httpResponse.IsSuccessStatusCode)
                {
                    throw new Exception("cannot retrieve users");
                }
                var content = await httpResponse.Content.ReadAsStringAsync();
                var lst = JsonConvert.DeserializeObject<mUserDetails>(content);
                string role = lst.userList[0].role;
                if (role == "Owner")
                {
                    ViewBag.RoleToChange = role;
                    return View(lst);
                }
                return View(lst);
            }
            return RedirectToAction("Index", "Home");
        }

        public IActionResult CreateUserPage()
        {
            
            return View();
        }

        public async Task<IActionResult> AssignRole(mUserDetails m)
        {
            //in Auth0 DB
            ClaimAccessToken();
            var content = JsonConvert.SerializeObject(m);
            var httpResponse = await _client.PostAsync(_assignRole,
                new StringContent(content, Encoding.Default, "application/json"));
            if ((int)httpResponse.StatusCode == 403)
            {
                return RedirectToAction("AccessDenied", "Home");
            }
            if (!httpResponse.IsSuccessStatusCode)
            {
                throw new Exception("cannot create user on Auth0");
            }
            return RedirectToAction("UsersList", "Users");
        }
        public async Task<IActionResult> CreateUser(mUserDetailsCreate newUser)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (ModelState.IsValid)
                {
                    //in Auth0 DB
                    ClaimAccessToken();
                    var content = JsonConvert.SerializeObject(newUser);
                    var httpResponse = await _client.PostAsync(_createUsers,
                        new StringContent(content, Encoding.Default, "application/json"));
                    if ((int)httpResponse.StatusCode == 403)
                    {
                        return RedirectToAction("AccessDenied", "Home");
                    }
                    if (!httpResponse.IsSuccessStatusCode)
                    {
                        throw new Exception("cannot create user on Auth0");
                    }
                    var createdTask =
                        JsonConvert.DeserializeObject<mUserDetailsCreate>(
                            await httpResponse.Content.ReadAsStringAsync()); 
                    
                    //in local DB
                    mUserInDB toSend = new mUserInDB();
                    toSend.User_Name = createdTask.name;
                    var x = createdTask.nickname;
                    var y = createdTask.family_name;

                    var content2 = JsonConvert.SerializeObject(toSend);
                    var httpResponse2 = await _client.PostAsync(_createUserInDB,
                        new StringContent(content2, Encoding.Default, "application/json"));
                    if (!httpResponse2.IsSuccessStatusCode)
                    {
                        throw new Exception("cannot create user on DB");
                    }
                    System.Threading.Thread.Sleep(2000);
                    return RedirectToAction("UsersList", "Users");
                }
            }
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> DeleteUser(string userID)
        {
            if (User.Identity.IsAuthenticated)
            {
                ClaimAccessToken();

                var httpResponse = await _client.DeleteAsync($"{_deleteUsers}/?userId={userID}");
                if ((int)httpResponse.StatusCode == 403)
                {
                    return RedirectToAction("AccessDenied", "Home");
                }
                if (!httpResponse.IsSuccessStatusCode)
                {
                    throw new Exception("cannot delete user with id : " + userID);
                }
            }
            return RedirectToAction("UsersList", "Users");
        }
        //Other methods
        private async void ClaimAccessToken()
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        }
    }
}
