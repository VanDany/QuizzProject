using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Quizz.Models;

namespace Quizz.Services.Interfaces
{
    public interface IQuizzService
    {
        public List<modQuizz> GetAllPublic();
        public List<modQuizz> GetALL();
        public modQuizz PutQuizz(modQuizz modQuizz);
        public modQuizz PostQuizz(modQuizz modQuizz);
        public modQuizz GetQuizzByID(int quizzID);
        public bool DeleteQuizz(int quizzID);
    }
}
