using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Authentication.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Сrowdfunding.Data;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using Сrowdfunding.CloudStorage;
using Сrowdfunding.Models;
using Сrowdfunding.Hubs;

namespace Сrowdfunding
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
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                        options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            }
            else //Production
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                        options.UseSqlServer(Configuration.GetConnectionString("DefaultConnectionProd")));
            }
            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddAuthentication()
                    .AddGoogle(options =>
                    {
                        IConfigurationSection googleAuthNSection =
                             Configuration.GetSection("Authentication:Google");
                        options.ClientId = "102145220985-utd9mhqgbamgu1tjiutlquis49t125bg.apps.googleusercontent.com";
                        options.ClientSecret = "o3paVzEAy8VRqH0RF0d8UKPt";
                    })
                    .AddTwitter(options =>
                    {
                        options.ConsumerKey = "9iGXRjBEfQ5v96uypzvKukgQ1";
                        options.ConsumerSecret = "TWiFCyDQh1pGqtoNruWW4tUcaVnDQ9cgJkSqmRlVIB3m34jOGA";
                        options.RetrieveUserDetails = true;
                    });
            services.AddControllersWithViews();
            services.AddRazorPages();
            services.AddSignalR();
            services.AddSingleton<ICloudStorage, CloudinaryStorage>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "reward",
                    pattern: "Home/Details/{id}/AddRewards",
                    defaults: new { controller = "Home", action = "Rewards"});
                endpoints.MapControllerRoute(
                    name: "support",
                    pattern: "Home/Details/{id}/Support",
                    defaults: new { controller = "Home", action = "Support" });
                
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapHub<CommentHub>("/CommentHub");
                endpoints.MapHub<NewsHub>("/NewsHub");
                endpoints.MapRazorPages();
            });
        }
    }
}
