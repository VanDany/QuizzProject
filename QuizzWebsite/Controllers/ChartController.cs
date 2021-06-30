using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using QuizzWebsite.Models;

namespace QuizzWebsite.Controllers
{
    public class ChartController : Controller
    {
        private readonly ILogger<ChartController> _logger;
        private readonly Random rnd = null;

        public ChartController(ILogger<ChartController> logger)
        {
            _logger = logger;
            rnd = new Random();
        }
        public IActionResult ChartOverview()
        {
            return View();
        }

        [HttpGet]
        public JsonResult ColumnChart()
        {
            var lstPopulation = GetColumnChartData();
            return Json(lstPopulation);
        }

        private List<mCategories> GetColumnChartData()
        {
            var lstChart = new List<mCategories>
            {
                new mCategories()
                {
                    Difficulty = "Facile",
                    Geographie = rnd.Next(10, 100),
                    SciencesEtNature = rnd.Next(10, 100),
                    Divertissement = rnd.Next(10, 100),
                    Histoire = rnd.Next(10, 100),
                    ArtEtLitterature = rnd.Next(10, 100),
                    Sports = rnd.Next(10, 100),
                },
                new mCategories()
                {
                    Difficulty = "Moyen",
                    Geographie = rnd.Next(10, 100),
                    SciencesEtNature = rnd.Next(10, 100),
                    Divertissement = rnd.Next(10, 100),
                    Histoire = rnd.Next(10, 100),
                    ArtEtLitterature = rnd.Next(10, 100),
                    Sports = rnd.Next(10, 100),
                },
                new mCategories()
                {
                    Difficulty = "Difficile",
                    Geographie = rnd.Next(10, 100),
                    SciencesEtNature = rnd.Next(10, 100),
                    Divertissement = rnd.Next(10, 100),
                    Histoire = rnd.Next(10, 100),
                    ArtEtLitterature = rnd.Next(10, 100),
                    Sports = rnd.Next(10, 100),
                }
            };
            return lstChart;
        }
    }
}
