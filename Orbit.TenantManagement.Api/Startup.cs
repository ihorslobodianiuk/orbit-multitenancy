using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Orbit.TenantManagement.Api.Database;
using Orbit.TenantManagement.Api.Infrastructure;

namespace Orbit.TenantManagement.Api;

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
        services.AddDbContext<TenantDbContext>(options =>
        {
            options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection"));
        });
        
        services.AddControllers();
        services.AddSwaggerGen();
        
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new AutoMapperProfile());
        });
        var mapper = config.CreateMapper();
        services.AddSingleton(mapper);
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
        
        app.UseRouting();
        app.UseAuthentication();  
        app.UseAuthorization();
        app.UseStaticFiles();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}