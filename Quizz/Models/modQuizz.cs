using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Quizz.Models
{
    public class modQuizz
    {
        [Key]
        public int QuizzID { get; set; }
        public DateTime CreationDate { get; set; }
        public string Title { get; set; }
        public string Category  { get; set; }
        public string Difficulty { get; set; }
        public int? AverageScore { get; set; }
        public int QuestionsCount { get; set; }


    }
}
