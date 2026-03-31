using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PositivityCoupon.Api.Data;
using PositivityCoupon.Api.Services;

namespace PositivityCoupon.Api
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
            services.AddControllers();
            services.AddDbContext<CouponAdminDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("CouponAdminDb")));
            services.AddScoped<AdminViewModelService>();
            services.AddCors(options =>
            {
                options.AddPolicy("Frontend", policy =>
                {
                    policy
                        .WithOrigins("http://localhost:4200")
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            using (var scope = app.ApplicationServices.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<CouponAdminDbContext>();
                dbContext.Database.EnsureCreated();
                SeedData.Initialize(dbContext);
            }

            app.UseRouting();
            app.UseCors("Frontend");

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}

