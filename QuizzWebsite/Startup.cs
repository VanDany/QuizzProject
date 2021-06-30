using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using QuizzWebsite.Middleware;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Session;
using Microsoft.IdentityModel.Tokens;

namespace QuizzWebsite
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient();
            //Sessions
            services.AddDistributedMemoryCache();
            services.AddSession(s => s.IdleTimeout = TimeSpan.FromMinutes(30));

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
           .AddCookie()
           .AddOpenIdConnect("Auth0", options =>
           {
               // Set the authority to your Auth0 domain
               options.Authority = $"https://{Configuration["Auth0:Domain"]}";
               // Configure the Auth0 Client ID and Client Secret
               options.ClientId = Configuration["Auth0:ClientId"];
               options.ClientSecret = Configuration["Auth0:ClientSecret"];
               // Set response type to code
               options.ResponseType = OpenIdConnectResponseType.Code;
               // Configure the scope
               options.Scope.Clear();
               options.Scope.Add("openid");
               options.Scope.Add("profile");
               options.Scope.Add("email");

               // Set the correct name claim type
               options.TokenValidationParameters = new TokenValidationParameters
               {
                   NameClaimType = "name"
               };

               //Scopes list
               options.Scope.Add("read:messages");
               options.Scope.Add("write:messages");
               options.Scope.Add("read:users");
               options.Scope.Add("write:users");

               options.CallbackPath = new PathString("/callback");
               options.ClaimsIssuer = "Auth0";
               options.SaveTokens = true;
               options.Events = new OpenIdConnectEvents
               {
                   OnRedirectToIdentityProvider = (context) =>
                   {
                       //Set the audience
                       context.ProtocolMessage.SetParameter("audience", Configuration["Auth0:Audience"]);
                       return Task.FromResult(0);
                   },
                   
                   OnMessageReceived = (context) =>
                   {
                       if (context.ProtocolMessage.Error == "access_denied")
                       {
                           context.HandleResponse();
                           context.Response.Redirect("/Account/AccessDenied");
                       }
                       return Task.FromResult(0);
                   },
                   
                   
                   OnRedirectToIdentityProviderForSignOut = (context) =>
                   {
                       var logoutUri = $"https://{Configuration["Auth0:Domain"]}/v2/logout?client_id={Configuration["Auth0:ClientId"]}";

                       var postLogoutUri = context.Properties.RedirectUri;
                       if (!string.IsNullOrEmpty(postLogoutUri))
                       {
                           if (postLogoutUri.StartsWith("/"))
                           {
                               // transform to absolute
                               var request = context.Request;
                               postLogoutUri = request.Scheme + "://" + request.Host + request.PathBase + postLogoutUri;
                           }
                           logoutUri += $"&returnTo={ Uri.EscapeDataString(postLogoutUri)}";
                       }

                       context.Response.Redirect(logoutUri);
                       context.HandleResponse();

                       return Task.CompletedTask;
                   }
               };
           });
            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //Session
            app.UseSession();
            app.UseLogRequestMiddleware();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseCookiePolicy();
            app.UseStaticFiles();
            
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute(
                    name: "questions",
                    pattern: "{controller=Home}/{action=Question}/{QuizzID}/{QuestionID}");
                endpoints.MapControllerRoute(
                    name: "deleteQuizz",
                    pattern: "{controller=API}/{action=DeleteQuizz}/{QuizzID}");
                endpoints.MapControllerRoute(
                    name: "quizzByID",
                    pattern: "{controller=API}/{action=QuizzById}/{QuizzID}");
                endpoints.MapControllerRoute(
                    name: "deleteUser",
                    pattern: "{controller=Users}/{action=DeleteUser}/{userID}");
            });
        }
    }
}
