using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Quizz.Context;
using Quizz.Models;
using Quizz.Services.Interfaces;
using QuizzWebsite.Models;

namespace Quizz.Services
{
    public class ProfileService : IProfileService
    {
        private readonly DBQuizzContext _db;

        public ProfileService(DBQuizzContext db)
        {
            _db = db;
        }

        public bool PostUserInDB(modUserInDB modUserInDb)
        {
            _db.Users.Add(modUserInDb);
            _db.SaveChanges();
            return true;
        }
        public bool PutScoreInDB(modUserInDB modUserInDb)
        {

            var user = _db.Users.FirstOrDefault(u => u.id == modUserInDb.id);
            if (user is not null)
                user.QuizzTot += modUserInDb.QuizzTot;
            user.WrongAnswersTot += modUserInDb.WrongAnswersTot;
            user.GoodAnswersTot += modUserInDb.GoodAnswersTot;
            _db.SaveChanges();
            return true;
        }

        public modUserInDB GetUserIdInDB(string username)
        {
            var lst = _db.Users.FirstOrDefault(d => d.User_Name == username);
            return lst;

        }

    }
}

