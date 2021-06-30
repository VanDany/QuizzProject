using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Quizz.Models;
using Quizz.Services.Interfaces;
using ILoggerManager = LoggerService.Interface.ILoggerManager;

namespace Quizz.Controllers
{
    [Route("question")]
    [ApiController]
    public class QuestionController : Controller
    {
        private readonly IQuestionService _questionService;
        private readonly ILoggerManager _logger;
        public QuestionController(IQuestionService questionService, ILoggerManager logger)
        {
            _questionService = questionService;
            _logger = logger;
        }
        /// <summary>
        /// Get questions' count
        /// </summary>
        /// <returns>Questions' count for a specific quizz</returns>
        /// <response code="200">Count retrieved</response>
        /// <response code="400">Something went wrong with the request</response>
        /// <response code="401">Unauthorized</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet("GetQuestionsCount")]
        public IActionResult GetQuestionsCount(int QuizzID)
        {
            try
            {
                var lst = _questionService.GetQuestionsCount(QuizzID);
                return Ok(lst);
            }
            catch
            {
                return BadRequest();
            }
        }
        /// <summary>
        /// Get Questions' list
        /// </summary>
        /// <returns>Questions' list for a specific quizz</returns>
        /// <response code="200">Questions retrieved</response>
        /// <response code="400">Something went wrong with the request</response>
        /// <response code="401">Unauthorized</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet("GetQuestions")]
        public IActionResult GetQuestions(int QuizzID, int QuestionID)
        {
            try
            {
                var lst = _questionService.GetQuestions(QuizzID, QuestionID);
                return Ok(lst);
            }
            catch
            {
                return BadRequest();
            }
        }
        /// <summary>
        /// Update Question
        /// </summary>
        /// <returns>Questions' list</returns>
        /// <response code="200">Questions to update retrieved</response>
        /// <response code="400">Something went wrong with the request</response>
        /// <response code="401">Unauthorized</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpPut("PutQuestion")]
        public IActionResult PutQuestion(modQuestion modQuestion)
        {
            try
            {
                var lst = _questionService.PutQuestion(modQuestion);
                return Ok(lst);
            }
            catch
            {
                return BadRequest();
            }
        }
        /// <summary>
        /// Create Questions' list
        /// </summary>
        /// <returns>Questions' list created</returns>
        /// <response code="200">Questions' list created</response>
        /// <response code="400">Something went wrong with the request</response>
        /// <response code="401">Unauthorized</response>
        [HttpPost("PostQuestions")]
        [Authorize("write:messages")]
        public IActionResult PostQuestions(modQuestion modQuestion)
        {
            try
            {
                var lst = _questionService.PostQuestions(modQuestion);
                return Ok(lst);
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
