using EPlast.BussinessLayer;
using EPlast.BussinessLayer.Interfaces;
using EPlast.DataAccess;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using EPlast.DataAccess.Repositories.Contracts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using EPlast.Models.ViewModelInitializations;
using EPlast.Models.ViewModelInitializations.Interfaces;

namespace EPlast
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
            services.AddDbContextPool<EPlastDBContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("EPlastDBConnection")));

            services.AddIdentity<User, IdentityRole>()
                    .AddEntityFrameworkStores<EPlastDBContext>()
                    .AddDefaultTokenProviders();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddScoped<IUserProfileRepository, UserProfileRepository>();
            services.AddScoped<INationalityRepository, NationalityRepository>();
            services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();
            services.AddScoped<IEducationRepository, EducationRepository>();
            services.AddScoped<IDegreeRepository, DegreeRepository>();
            services.AddScoped<IReligionRepository, ReligionRepository>();
            services.AddScoped<IGenderRepository, GenderRepository>();
            services.AddScoped<IWorkRepository, WorkRepository>();
            services.AddScoped<IApproverRepository, ApproverRepository>();
            services.AddScoped<IConfirmedUserRepository, ConfirmedUserRepository>();

            services.AddScoped<IDocumentTemplateRepository, DocumentTemplateRepository>();
            services.AddScoped<IDecesionStatusRepository, DecesionStatusRepository>();
            services.AddScoped<IDecesionTargetRepository, DecesionTargetRepository>();
            services.AddScoped<IOrganizationRepository, OrganizationRepository>();
            services.AddScoped<IDecesionRepository, DecesionRepository>();
            services.AddScoped<IEmailConfirmation, EmailConfirmation>();
            services.AddScoped<IAnnualReportVMInitializer, AnnualReportVMInitializer>();

            services.Configure<IdentityOptions>(options =>
            {
                options.SignIn.RequireConfirmedEmail = true;
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireUppercase = false;
                options.User.RequireUniqueEmail = true;
                options.Password.RequireNonAlphanumeric = false;

                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                options.Lockout.MaxFailedAccessAttempts = 10;
            });
            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.HttpOnly = true;
                options.Cookie.Expiration = TimeSpan.FromDays(5);
                options.LoginPath = "/Account/LoginAndRegister";
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStatusCodePagesWithReExecute("/Error/HandleError", "?code={0}");
            app.UseStaticFiles();
            app.UseDefaultFiles();
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}