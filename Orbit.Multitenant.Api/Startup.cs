// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.EntityFrameworkCore;
using Orbit.Multitenant.Api.Database;
using Orbit.Multitenant.Api.Multitenancy.Database;
using Orbit.Multitenant.Api.Multitenancy.Database.Interceptors;
using Orbit.Multitenant.Api.Multitenancy.Middleware;

namespace Orbit.Multitenant.Api
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
            // Register Scoped DbContexts:
            services
                // Register the Tenant Database:
                .AddDbContext<TenantDbContext>(options => options.UseNpgsql("Host=localhost;Port=5432;Database=sampledb;Pooling=false;User Id=app_user;Password=app_user;"))
                // Register the Application Database:
                .AddDbContext<ApplicationDbContext>(options => options
                    .AddInterceptors(new PostgresTenantDbConnectionInterceptor())
                    .UseNpgsql("Host=localhost;Port=5432;Database=sampledb;Pooling=false;User Id=app_user;Password=app_user;"));

            services.AddControllers();
            services.AddSwaggerGen();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();

            app.UseMultiTenant();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
