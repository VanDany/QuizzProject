using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizzWebsite.Models
{
    public class mUserInDB
    {
        public int id { get; set; }
        public string User_Name { get; set; }
        public int? GoodAnswersTot { get; set; }
        public int? WrongAnswersTot { get; set; }
        public int? QuizzTot { get; set; }
    }
}
