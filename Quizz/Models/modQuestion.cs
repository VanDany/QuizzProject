using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Quizz.Models
{
    public class modQuestion
    {
        [Key]
        public int QuestionID { get; set; }
        public int QuizzID { get; set; }
        public string Question { get; set; }
        public bool Answer { get; set; }
    }
}
