using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Quizz.Context;
using Quizz.Models;
using Quizz.Services.Interfaces;

namespace Quizz.Services
{
    public class QuizzService :IQuizzService
    {
        private readonly DBQuizzContext _db;
        public QuizzService(DBQuizzContext db)
        {
            _db = db;
        }
        public List<modQuizz> GetAllPublic()
        {
            var lst = _db.QuizzList.OrderByDescending(c => c.CreationDate).ToList();
            return lst;
        }
        public List<modQuizz> GetALL()
        {
            var lst = _db.QuizzList.OrderByDescending(c => c.CreationDate).ToList();
            return lst;
        }
        public modQuizz PostQuizz(modQuizz modQuizz)
        {
            _db.Add(modQuizz);
            _db.SaveChanges();
            return modQuizz;
        }

        public modQuizz PutQuizz(modQuizz modQuizz)
        {
            _db.Update(modQuizz);
            _db.SaveChanges();
            return modQuizz;
        }
        public modQuizz GetQuizzByID(int quizzID)
        {
            var lst = _db.QuizzList.FirstOrDefault(c => c.QuizzID == quizzID);
            return lst;
        }
        public bool DeleteQuizz(int quizzID)
        {
            //var test = _db.QuestionList.Where(m => m.QuizzID == quizzID).ToList();
            var test2 = _db.QuizzList.FirstOrDefault(x => x.QuizzID == quizzID);
            //_db.QuestionList.RemoveRange(test);
            _db.QuizzList.Remove(test2);
            _db.SaveChanges();
            return true;
        }
        //public bool GetUserID(modUserInDB modUserInDb)
        //{
        //    _db.Users.Update
        //}


    }
}
