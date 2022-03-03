using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Reservation.Middleware;
using Reservation.Models.ConfigModel;
using Reservation.Services;
using Reservation.Services.Interfaces;

namespace Reservation
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            // kullanýlacak interfaceler için instance tanýmlarýnýn yapýlmasý
            services.AddBrowserDetection();
            services.AddControllersWithViews();
            services.AddScoped<ISessionService, SessionService>();
            services.AddScoped<ILocationService, LocationService>();
            services.AddScoped<IJourneyService, JourneyService>();
            services.AddHttpClient();
            services.AddHttpContextAccessor();
            services.AddSession();
            services.Configure<ApiConfig>(Configuration.GetSection("API"));
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
            app.UseSession();
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            // Hata yönetimi için kullanýlacak olan middleware katmanýnýn eklenmesi
            app.UseMiddleware<ErrorHandlerMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
