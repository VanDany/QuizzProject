using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Quizz.Models;

namespace Quizz.Services.Interfaces
{
    public interface IProfileService
    {
        public modUserInDB GetUserIdInDB(string username);
        public bool PostUserInDB(modUserInDB modUserInDb);
        public bool PutScoreInDB(modUserInDB modUserInDb);
    }
}
