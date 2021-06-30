using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quizz.Models
{
    public class modUserDetailsCreate
    {
        public string connection { get; set; }
        public string email { get; set; }
        public string name { get; set; }
        public string password { get; set; }
    }
}
