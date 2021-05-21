using CrowdFundingProject.Data;
using CrowdFundingProject.Data.Interfaces;
using CrowdFundingProject.Data.Repository;
using CrowdFundingProject.Interfaces;
using CrowdFundingProject.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrowdFundingProject
{
    public class Startup
    {
        private IConfigurationRoot ICRoot;

        [Obsolete]
        public Startup(Microsoft.AspNetCore.Hosting.IHostingEnvironment configuration)
        {
            ICRoot = new ConfigurationBuilder().SetBasePath(configuration.ContentRootPath).AddJsonFile("dbsettings.json").Build();

        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
               options.UseSqlServer(ICRoot.GetConnectionString("DefaultConnection")));
            services.AddTransient<ICompanyRepository, CompanyRepository>();
            services.AddTransient<ICategoryRepository, CategoryRepository>();
            services.AddIdentity<User, IdentityRole>(opts =>
            {
                opts.Password.RequiredLength = 0;
                opts.Password.RequireDigit = false;
                opts.Password.RequireUppercase = false;
                opts.Password.RequireNonAlphanumeric = false;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>();

            //services.AddAuthentication();
            //    //.AddGoogle(options =>
            //    //{
            //    //    options.ClientId = Configuration["App:GoogleClientId"];
            //    //    options.ClientSecret = Configuration["App:GoogleClientSecret"];
            //    //})
                
            services.AddControllersWithViews();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
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
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
