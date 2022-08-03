using Microsoft.EntityFrameworkCore;
using Orbit.Multitenant.Api.Database;
using Orbit.Multitenant.Api.Middleware;

namespace Orbit.Multitenant.Api;

public class Startup
{
    public IConfiguration Configuration { get; }
    
    public Startup(IWebHostEnvironment env)
    {
        Configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .AddJsonFile($"appsettings.{env.EnvironmentName}.json")
            .AddEnvironmentVariables()
            .AddUserSecrets<Startup>()
            .Build();
    }
    
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddDbContext<OrbitDbContext>(options =>
        {
            options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection"));
        });

        services.AddScoped<IDomainContextInfo, DomainContextInfo>();
            
        services.AddControllers();
        services.AddSwaggerGen();
    }
    
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
        app.UseAuthorization();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}