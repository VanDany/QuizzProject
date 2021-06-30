using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Quizz.Services;
using Quizz.Services.Interfaces;

namespace Quizz.Configuration
{
    public static class Configuration
    {
        public static void UseServices(this IServiceCollection services)
        {
            services.AddScoped<IQuizzService, QuizzService>();
            services.AddScoped<IQuestionService, QuestionService>();
            services.AddScoped<IProfileService, ProfileService>();
        }
    }
}
