using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Online_MarketPlace_System.Data;
using System;

namespace Online_MarketPlace_System
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
            services.AddDbContext<MarketplaceDbContext>(options =>
             options.UseMySql(Configuration.GetConnectionString("MarketplaceConnectionString1"), new MySqlServerVersion(new Version())));

            services.AddDbContext<MarketDbContext>(options =>
             options.UseMySql(Configuration.GetConnectionString("MarketplaceConnectionString2"), new MySqlServerVersion(new Version())));

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme,
            options =>
            {
                options.LoginPath = new PathString("/User/Login");

            });
            //services.Add(new ServiceDescriptor(typeof(MarketplaceDbContext), new MarketplaceDbContext(Configuration.GetConnectionString("MarketplaceConnectionString"))));

            services.AddControllersWithViews();
            services.AddSession(options => {
                options.IdleTimeout = TimeSpan.FromMinutes(240);//You can set Time (4hrs)   
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
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
            app.UseSession();
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
