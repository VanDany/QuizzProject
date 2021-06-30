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
    [Route("quizz")]
    [ApiController]
    public class QuizzController : Controller
    {

        //POUR LECTURE DANS DB
        private readonly IQuizzService _quizzService;
        private readonly ILoggerManager _logger;
        public QuizzController(IQuizzService quizzService, ILoggerManager logger)
        {
            _quizzService = quizzService;
            _logger = logger;
        }


        /// <summary>
        /// Get all quizz with Authorization
        /// </summary>
        /// <returns>A list with all quizz</returns>
        /// <response code="200">Returns all the quizz</response>
        /// <response code="400">Something went wrong with the request</response>
        /// <response code="401">Unauthorized</response>
        [HttpGet("GetAll")]
        [Authorize("read:messages")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult GetALL()
        {
            try
            {
                var lst = _quizzService.GetALL();
                return Ok(lst);
            }
            catch
            {
                return BadRequest();
            }
        }
        /// <summary>
        /// Get quizz by ID
        /// </summary>
        /// <returns>a quizz by its id</returns>
        /// <response code="200">Returns a quizz by its id</response>
        /// <response code="400">Something went wrong with the request</response>
        /// <response code="401">Unauthorized</response>
        [HttpGet("GetQuizzByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        //[Route("~/GetQuizzByID/{quizzID}")]
        public IActionResult GetQuizzByID(int quizzID)
        {
            try
            {
                var lst = _quizzService.GetQuizzByID(quizzID);
                return Ok(lst);
            }
            catch
            {
                return BadRequest();
            }
        }
        /// <summary>
        /// Get all quizz without Authorization
        /// </summary>
        /// <returns>A list with all quizz</returns>
        /// <response code="200">Returns all the quizz</response>
        /// <response code="400">Something went wrong with the request</response>
        /// <response code="401">Unauthorized</response>
        [HttpGet("GetAllPublic")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult GetAllPublic()
        {
            _logger.LogInfo("Fetching all quizz from db.");
            var lst = _quizzService.GetAllPublic();
            _logger.LogInfo($"Returning {lst.Count} quizz.");
            return Ok(lst);
            }
        /// <summary>
        /// Post a quizz
        /// </summary>
        /// <returns>Questions form</returns>
        /// <response code="200">Quizz registered in DB</response>
        /// <response code="400">Something went wrong with the request</response>
        /// <response code="401">Unauthorized</response>
        [HttpPost("PostQuizz")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Authorize("write:messages")]
        public IActionResult PostQuizz(modQuizz modQuizz)
        {
            try
            {
                var lst = _quizzService.PostQuizz(modQuizz);
                return Ok(lst);
            }
            catch
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Update a quizz
        /// </summary>
        /// <returns>quizz updated</returns>
        /// <response code="200">Quizz updated</response>
        /// <response code="400">Something went wrong with the request</response>
        /// <response code="401">Unauthorized</response>
        [HttpPut("PutQuizz")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Authorize("write:messages")]
        public IActionResult PutQuizz(modQuizz modQuizz)
        {
            try
            {
                var lst = _quizzService.PutQuizz(modQuizz);
                return Ok(lst);
            }
            catch
            {
                return BadRequest();
            }
        }


        /// <summary>
        /// Delete a quizz by its ID
        /// </summary>
        /// <returns>a quizz by its id</returns>
        /// <response code="200">Delete a quizz by its id</response>
        /// <response code="400">Something went wrong with the request</response>
        /// <response code="401">Unauthorized</response>
        [HttpDelete("DeleteQuizz")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        //[Authorize("write:messages")]
        public IActionResult DeleteQuizz(int quizzID)
        {
            try
            {
                var lst = _quizzService.DeleteQuizz(quizzID);
                return Ok(lst);
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
