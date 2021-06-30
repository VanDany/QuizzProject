using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Quizz.Context;
using Quizz.Models;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Quizz.Services.Interfaces;
using ILoggerManager = LoggerService.Interface.ILoggerManager;

namespace Quizz.Controllers
{
    [Produces("application/json")]
    [Route("profile")]
    [ApiController]
    public class ProfileController : Controller
    {
        private readonly IProfileService _profileService;
        private readonly ILoggerManager _logger;
        public ProfileController(IProfileService profileService, ILoggerManager logger)
        {
            _profileService = profileService;
            _logger = logger;
        }
        /// <summary>
        /// Post a user in local DB
        /// </summary>
        /// <returns>user created in local DB</returns>
        /// <response code="200">User created in DB</response>
        /// <response code="400">Something went wrong with the request</response>
        /// <response code="401">Unauthorized</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpPost("PostUserInDB")]
        [Authorize]
        public IActionResult PostUserInDB(modUserInDB modUserInDb)
        {
            try
            {
                var insert = _profileService.PostUserInDB(modUserInDb);
                return Ok(insert);
            }
            catch
            {
                return BadRequest();
            }
        }
        /// <summary>
        /// Score updated in local DB
        /// </summary>
        /// <returns>Score updated</returns>
        /// <response code="200">Score udpated in local DB</response>
        /// <response code="400">Something went wrong with the request</response>
        /// <response code="401">Unauthorized</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpPost("PutScoreInDB")]
        [Authorize]
        public IActionResult PutScoreInDB(modUserInDB modUserInDb)
        {
            try
            {
                var insert = _profileService.PutScoreInDB(modUserInDb);
                return Ok(insert);
            }
            catch
            {
                return BadRequest();
            }
        }
        /// <summary>
        /// Get user in local DB
        /// </summary>
        /// <returns>User's information</returns>
        /// <response code="200">User retrieved in local DB</response>
        /// <response code="400">Something went wrong with the request</response>
        /// <response code="401">Unauthorized</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet("GetUserInDB")]
        [Authorize]
        public IActionResult GetUserInDB(string username)
        {
            try
            {
                var lst = _profileService.GetUserIdInDB(username);
                return Ok(lst);
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
