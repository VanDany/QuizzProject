using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Channels;
using System.Threading.Tasks;
using Quizz.Models;

namespace Quizz.Context
{
    public class DBQuizzContext : DbContext
    {
        public DBQuizzContext()
        {

        }
        public DBQuizzContext(DbContextOptions<DBQuizzContext>options):base(options)
        {

        }
        public DbSet<modQuizz> QuizzList { get; set; }
        public DbSet<modQuestion> QuestionList { get; set; }
        public DbSet<modUserInDB> Users { get; set; }
    }
}
