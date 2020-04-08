﻿using EPlast.BussinessLayer;
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
using System.Threading.Tasks;
using System.Linq;
using EPlast.Models.ViewModelInitializations.Interfaces;
using EPlast.Models.ViewModelInitializations;
using EPlast.BussinessLayer.Settings;
using EPlast.BussinessLayer.AccessManagers;
using EPlast.BussinessLayer.AccessManagers.Interfaces;

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
            services.AddOptions();
            services.AddDbContextPool<EPlastDBContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("EPlastDBConnection")));
            services.AddIdentity<User, IdentityRole>()
                    .AddEntityFrameworkStores<EPlastDBContext>()
                    .AddDefaultTokenProviders();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Admin",
                    authBuilder =>
                    {
                        authBuilder.RequireRole("Admin");
                    });
            });

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
            services.AddScoped<IDecesionTargetRepository, DecesionTargetRepository>();
            services.AddScoped<IOrganizationRepository, OrganizationRepository>();
            services.AddScoped<IDecesionRepository, DecesionRepository>();
            services.AddScoped<IEmailConfirmation, EmailConfirmation>();
            services.AddScoped<IAnnualReportVMInitializer, AnnualReportVMInitializer>();
            services.AddScoped<IViewAnnualReportsVMInitializer, ViewAnnualReportsVMInitializer>();
            services.AddScoped<IDecisionVMIitializer, DecisionVMIitializer>();
            services.AddScoped<IPDFService, PDFService>();
            services.AddScoped<ICityAccessManagerSettings, CityAccessManagerSettings>();
            services.AddScoped<ICityAccessManager, CityAccessManager>();
            services.Configure<EmailServiceSettings>(Configuration.GetSection("EmailServiceSettings"));
            services.Configure<IdentityOptions>(options =>
            {
                options.SignIn.RequireConfirmedEmail = true;
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireUppercase = false;
                options.User.RequireUniqueEmail = true;
                options.Password.RequireNonAlphanumeric = false;

                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
            });

            services.AddAuthentication()
                .AddGoogle(options =>
                {
                    options.ClientId = Configuration.GetSection("GoogleAuthentication:GoogleClientId").Value;
                    options.ClientSecret = Configuration.GetSection("GoogleAuthentication:GoogleClientSecret").Value;
                })
                .AddFacebook(options =>
                {
                    options.AppId = Configuration.GetSection("FacebookAuthentication:FacebookAppId").Value;
                    options.AppSecret = Configuration.GetSection("FacebookAuthentication:FacebookAppSecret").Value;
                });

            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.HttpOnly = true;
                options.Cookie.Expiration = TimeSpan.FromDays(5);
                options.LoginPath = "/Account/Login";
                options.LogoutPath = "/Account/Logout";
            });
        }

        private async Task CreateRoles(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
            var roles = new[] { "Admin", "Прихильник", "Пластун", "Голова Пласту","Адміністратор подій", "Голова Куреня","Діловод Куреня",
            "Голова Округу","Діловод Округу","Голова Станиці","Діловод Станиці"};
            foreach (var role in roles)
            {
                if (!(await roleManager.RoleExistsAsync(role)))
                {
                    var idRole = new IdentityRole
                    {
                        Name = role
                    };

                    var res = await roleManager.CreateAsync(idRole);
                }
            }
            var admin = Configuration.GetSection("Admin");
            var profile = new User
            {
                Email = admin["Email"],
                UserName = admin["Email"],
                FirstName = "Admin",
                LastName = "Admin",
                EmailConfirmed = true,
                ImagePath = "default.png",
                UserProfile = new UserProfile()
            };
            if (await userManager.FindByEmailAsync(admin["Email"]) == null)
            {
                var res = await userManager.CreateAsync(profile, admin["Password"]);
                if (res.Succeeded)
                    await userManager.AddToRoleAsync(profile, "Admin");
            }
            else if (!await userManager.IsInRoleAsync(userManager.Users.First(item => item.Email == profile.Email), "Admin"))
            {
                var user = userManager.Users.First(item => item.Email == profile.Email);
                await userManager.AddToRoleAsync(user, "Admin");
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider services)
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
            CreateRoles(services).Wait();
        }
    }
}