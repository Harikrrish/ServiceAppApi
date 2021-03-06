using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ServiceAppApi.Models;
using ServiceAppApi.Repositories.UnitOfWork;
using ServiceAppApi.Services.AccountService;
using ServiceAppApi.Services.CommonService;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ServiceAppApi
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
            services.AddControllersWithViews();
            services.AddMvc();
            services.AddDbContext<ServicesAppDataContext>(options => options.UseSqlServer(Configuration["ConnectionStrings:SqlConnection"]));
            services.AddScoped<ICommonService, CommonService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IRepository<Product>, RepositoryBase<Product>>();
            services.AddScoped<IRepository<PartyRole>, RepositoryBase<PartyRole>>();
            services.AddScoped<IRepository<Role>, RepositoryBase<Role>>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            #region CORS Configuration

            List<string> url = new List<string>()
            {
                "http://localhost:3000", 
                
                "http://localhost:49662",     
            };

            app.UseCors(
                options => url.ForEach(u => options.WithOrigins(u).AllowAnyHeader().AllowAnyMethod())
            );

            #endregion

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
            app.UseStaticFiles();

            app.UseRouting();

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
