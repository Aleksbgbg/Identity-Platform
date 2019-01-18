﻿namespace Identity.Platform
{
    using Identity.Platform.Middleware;
    using Identity.Platform.Models;
    using Identity.Platform.Models.Database;
    using Identity.Platform.Models.Repositories;

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

            services.AddIdentity<AppUser, IdentityRole>(options => options.User.AllowedUserNameCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+-._@/ ")
                    .AddEntityFrameworkStores<AppIdentityDbContext>()
                    .AddDefaultTokenProviders();

            services.AddAuthentication()
                    .AddGoogle(options =>
                    {
                        options.ClientId = _configuration["Secrets:GoogleApi:ClientId"];
                        options.ClientSecret = _configuration["Secrets:GoogleApi:ClientSecret"];
                    });

            services.AddTransient<IUserInfoRepository, UserInfoRepository>();

            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseStatusCodePages();
            }

            app.UseMiddleware<BackupUserImageMiddleware>();

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