using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Quizz.Context;
using Quizz.Handler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;
using FluentValidation;
using FluentValidation.AspNetCore;
using LoggerService;
using NLog;
using Quizz.Configuration;
using Quizz.Extensions;
using Quizz.Models;
using Quizz.Services;
using Quizz.Services.Interfaces;
using Quizz.Validator;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace Quizz
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            LogManager.LoadConfiguration(String.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().AddFluentValidation();
            services.AddControllers();
            //Add DB Context
            services.AddDbContext<DBQuizzContext>(options =>
            options.UseSqlServer(
                Configuration.GetConnectionString("DefaultConnection")));

            //Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo 
                {
                    Version = "v1",
                    Title = "Quizz API",
                    Description = "API for my Website VRAI OU FAUX - first version",
                    Contact = new OpenApiContact
                    {
                        Name = "Vanmuylder Dany",
                        Email = "danydynamo@gmail.com",
                    }
                });
                c.SwaggerDoc("v2", new OpenApiInfo
                {
                    Version = "v2",
                    Title = "Quizz API",
                    Description = "API for my Website VRAI OU FAUX - second version",
                    Contact = new OpenApiContact
                    {
                        Name = "Vanmuylder Dany",
                        Email = "danydynamo@gmail.com",
                    }
                });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
            //Add Authentication with Auth0
            var domain = $"https://{Configuration["Auth0:Domain"]}/";

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.Authority = domain;
                options.Audience = Configuration["Auth0:Audience"];
            });
            //Add Authorization with Auth0
            services.AddAuthorization(options =>
            {
                options.AddPolicy("read:messages", policy => policy.Requirements.Add(new HasScopeRequirement("read:messages", domain)));
                options.AddPolicy("write:messages", policy => policy.Requirements.Add(new HasScopeRequirement("write:messages", domain)));
                options.AddPolicy("read:users", policy => policy.Requirements.Add(new HasScopeRequirement("read:users", domain)));
                options.AddPolicy("write:users", policy => policy.Requirements.Add(new HasScopeRequirement("write:users", domain)));
            });
            services.AddSingleton<IAuthorizationHandler, HasScopeHandler>();
            services.UseServices();

            //FluentValidation injection
            services.AddTransient<IValidator<modQuizz>, InsertQuizzValidator>();

            //External logger injection
            services.AddSingleton<LoggerService.Interface.ILoggerManager, LoggerManager>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, LoggerService.Interface.ILoggerManager logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Quizz v1");
                    c.SwaggerEndpoint("/swagger/v2/swagger.json", "Quizz v2");
                });
            }
            //custom middelware to manage exception
            app.ConfigureCustomExceptionMiddleware();
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
