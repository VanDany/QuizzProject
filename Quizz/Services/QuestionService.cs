using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Quizz.Context;
using Quizz.Models;
using Quizz.Services.Interfaces;

namespace Quizz.Services
{
    public class QuestionService : IQuestionService
    {
        private readonly DBQuizzContext _db;
        public QuestionService(DBQuizzContext db)
        {
            _db = db;
        }
        public int GetQuestionsCount(int QuizzID)
        {
            var count = _db.QuizzList.Where(c => c.QuizzID == QuizzID).Select(d => d.QuestionsCount).FirstOrDefault();
            return count;
        }
        public modQuestion GetQuestions(int QuizzID, int QuestionID)
        {
            var lst = _db.QuestionList.FirstOrDefault(b => b.QuestionID == QuestionID && b.QuizzID == QuizzID);
            return lst;
        }
        public bool PostQuestions(modQuestion modQuestion)
        {
            var ok = _db.Add(modQuestion);
            _db.SaveChanges();
            return true;
        }
        public bool PutQuestion(modQuestion modQuestion)
        {
            _db.Update(modQuestion);
            _db.SaveChanges();
            return true;
        }
    }
}
