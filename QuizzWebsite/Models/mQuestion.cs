using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QuizzWebsite.Models
{
    public class mQuestion
    {
        [Key]
        public int QuestionID { get; set; }
        public bool Answer { get; set; }
        public string Question { get; set; }
        public int QuizzID { get; set; }
    }
}
