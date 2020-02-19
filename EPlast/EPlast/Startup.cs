using EPlast.DataAccess;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContextPool<EPlastDBContext>(options => 
                options.UseSqlServer(Configuration.GetConnectionString("EPlastDBConnection")));
            services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<EPlastDBContext>();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddScoped<INationalityRepository, NationalityRepository>();
            services.AddScoped<IEventRepository, EventRepository>();
            services.AddScoped<IGallaryRepository, GallaryRepository>();
            services.AddScoped<IParticipantStatusRepository, ParticipantStatusRepository>();
            services.AddScoped<IParticipantRepository, ParticipantRepository>();
            services.AddScoped<IEventCategoryRepository, EventCategoryRepository>();
            services.AddScoped<ISubEventCategoryRepository, SubEventCategoryRepository>();
            services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();


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
