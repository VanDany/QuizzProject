using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Quizz.Models;

namespace Quizz.Services.Interfaces
{
    public interface IQuestionService
    {
        public modQuestion GetQuestions(int QuizzID, int QuestionID);
        public bool PutQuestion(modQuestion modQuestion);
        public bool PostQuestions(modQuestion modQuestion);
        public int GetQuestionsCount(int QuizzID);
    }
}
