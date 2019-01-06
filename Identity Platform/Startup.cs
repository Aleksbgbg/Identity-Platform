namespace Identity.Platform
{
    using Identity.Platform.Controllers;
    using Identity.Platform.Models;
    using Identity.Platform.Models.Database;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    internal class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AppIdentityDbContext>(options => options.UseNpgsql(_configuration["Data:Identity:ConnectionString"]));

            services.AddIdentity<AppUser, IdentityRole>()
                    .AddEntityFrameworkStores<AppIdentityDbContext>()
                    .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(options => options.LoginPath = $"/Authorization/{nameof(AuthorizationController.Login)}");

            services.AddAuthentication()
                    .AddGoogle(options =>
                    {
                        options.ClientId = _configuration["Secrets:GoogleApi:ClientId"];
                        options.ClientSecret = _configuration["Secrets:GoogleApi:ClientSecret"];
                    });

            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseStatusCodePages();
            }

            app.UseAuthentication();
            app.UseStaticFiles();
            app.UseMvc(routes =>
            {
                routes.MapRoute(name: null,
                                template: "{Controller=Home}/{Action=Index}");
            });
        }
    }
}