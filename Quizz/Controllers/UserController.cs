using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Quizz.Models;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Quizz.Auth0Token;
namespace Quizz.Controllers
{
    [Route("user")]
    [ApiController]
    public class UserController : Controller
    {
        /// <summary>
        /// Get Users
        /// </summary>
        /// <returns>Users' list</returns>
        /// <response code="200">Users' list found</response>
        /// <response code="400">Something went wrong with the request</response>
        /// <response code="401">Unauthorized</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet("GetUsers")]
        [Authorize("read:users")]
        public async Task<ActionResult<modUserDetails>> GetUsers()
        {
            var accessToken = await Auth0Token.Auth0Token.ClaimAccessTokenAuth0();

            var uClient = new RestClient("https://dev-hdysdy74.eu.auth0.com/api/v2/users");
            var uRequest = new RestRequest(Method.GET);
            uRequest.AddHeader("authorization", "Bearer " + accessToken);
            var uResponse = await uClient.ExecuteAsync(uRequest);
            var message = uResponse.Content;
            var users = new modUserDetails();
            users.userList = JsonConvert.DeserializeObject<List<modUserDetails>>(message);
            //GET ROLES
            foreach (var item in users.userList)
            {
                var client = new RestClient("https://dev-hdysdy74.eu.auth0.com/api/v2/users/" + item.user_id+"/roles");
                var request = new RestRequest(Method.GET);
                request.AddHeader("authorization", "Bearer " + accessToken);
                IRestResponse response = await client.ExecuteAsync(request);
                var lst = response.Content;
                var essai = JsonConvert.DeserializeObject<List<modRole>>(lst);
                if (essai.Count != 0)
                {
                    item.role = essai[0].name;
                }
            }
            return users;
        }
        /// <summary>
        /// Get Role
        /// </summary>
        /// <returns>the role's name</returns>
        /// <response code="200">Role's name found</response>
        /// <response code="400">Something went wrong with the request</response>
        /// <response code="401">Unauthorized</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Route("~/GetRole/{userID}")]
        [Authorize("read:users")]
        public async Task<string> GetRole(string userID)
        {
            var accessToken = await Auth0Token.Auth0Token.ClaimAccessTokenAuth0();

            var client = new RestClient($"https://YOUR_DOMAIN/api/v2/users/{userID}/roles");
            var request = new RestRequest(Method.GET);
            request.AddHeader("authorization", "Bearer " + accessToken);
            IRestResponse response = await client.ExecuteAsync(request);
            var lst = response.Content;
            return lst;
        }
        /// <summary>
        /// Create User
        /// </summary>
        /// <returns>the role's name</returns>
        /// <response code="200">User is created</response>
        /// <response code="400">Something went wrong with the request</response>
        /// <response code="401">Unauthorized</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpPost("PostUsers")]
        [Authorize("write:users")]
        public async Task<ActionResult<modUserDetailsCreate>> PostUsers(modUserDetailsCreate newUser)
        {
            var accessToken = await Auth0Token.Auth0Token.ClaimAccessTokenAuth0();
            //POST USER on Auth0 database. U for User
            var uClient = new RestClient("https://dev-hdysdy74.eu.auth0.com/api/v2/users");
            var uRequest = new RestRequest(Method.POST);
            uRequest.AddHeader("authorization", "Bearer " + accessToken);
            uRequest.AddParameter("application/json", JsonConvert.SerializeObject(newUser), ParameterType.RequestBody);
            var uResponse = await uClient.ExecuteAsync(uRequest);
            return newUser;
        }
        /// <summary>
        /// Update User
        /// </summary>
        /// <returns>user updated</returns>
        /// <response code="200">User is updated</response>
        /// <response code="400">Something went wrong with the request</response>
        /// <response code="401">Unauthorized</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpPatch("PutUsers")]
        [Authorize]
        public async Task<ActionResult<modUserDetailsCreate>> PutUsers(string userId, modUserDetailsCreate changeUser)
        {
            var accessToken = await Auth0Token.Auth0Token.ClaimAccessTokenAuth0();
            var client = new RestClient("https://dev-hdysdy74.eu.auth0.com/api/v2/users" + "/" + userId);
            var request = new RestRequest(Method.PATCH);
            request.AddHeader("authorization", "Bearer " + accessToken);
            request.AddHeader("content-type", "application/json");
            request.AddParameter("application/json", JsonConvert.SerializeObject(changeUser), ParameterType.RequestBody);
            var uResponse = await client.ExecuteAsync(request);
            return changeUser;
        }
        /// <summary>
        /// Delete a user
        /// </summary>
        /// <returns>user ID deleted</returns>
        /// <response code="200">User_id updated</response>
        /// <response code="400">Something went wrong with the request</response>
        /// <response code="401">Unauthorized</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpDelete("DeleteUsers")]
        [Authorize]
        public async Task<string> DeleteUsers(string userId)
        {
            var accessToken = await Auth0Token.Auth0Token.ClaimAccessTokenAuth0();
            //POST USER on Auth0 database. U for User
            var uClient = new RestClient("https://dev-hdysdy74.eu.auth0.com/api/v2/users" + "/" + userId);
            var uRequest = new RestRequest(Method.DELETE);
            uRequest.AddHeader("authorization", "Bearer " + accessToken);
            uRequest.AddParameter("application/json", JsonConvert.SerializeObject(userId), ParameterType.RequestBody);

            var uResponse = await uClient.ExecuteAsync(uRequest);
            return userId;
        }
        /// <summary>
        /// Assign a role to a user
        /// </summary>
        /// <returns>User_id with the role assigned</returns>
        /// <response code="200">Role assigned</response>
        /// <response code="400">Something went wrong with the request</response>
        /// <response code="401">Unauthorized</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpPost("AssignRole")]
        [Authorize]
        public async Task<string> AssignRole(modUserDetails m)
        {
            string roleID;
            switch (m.role)
            {
                case "Admin":
                    roleID = "rol_YWJvTA1meG2yWW6z";
                    break;
                case "Owner":
                    roleID = "rol_LhPYYLpr6ygb67Rn";
                    break;
                case "User":
                    roleID = "rol_4YqZFVlhgLwnGGX4";
                    break;
                case "Agent":
                    roleID = "rol_Gt7htfibDj4i3Qw5";
                    break;
                default:
                    roleID = "";
                    break;
            }
                var accessToken = await Auth0Token.Auth0Token.ClaimAccessTokenAuth0();
            var client = new RestClient("https://dev-hdysdy74.eu.auth0.com/api/v2/users/" + m.user_id + "/roles");
            var request = new RestRequest(Method.POST);
            request.AddHeader("content-type", "application/json");
            request.AddHeader("authorization", "Bearer " + accessToken);
            request.AddHeader("cache-control", "no-cache");
            request.AddParameter("application/json", "{ \"roles\": [ \"" + roleID + "\", \"" + roleID + "\" ] }", ParameterType.RequestBody);
            var response = client.Execute(request);
            return m.user_id;
        }
    }
}
