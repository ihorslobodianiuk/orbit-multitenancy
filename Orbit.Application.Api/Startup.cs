using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Orbit.Application.Api.Database;
using Orbit.Application.Api.Infrastructure;
using Orbit.Application.Api.Middleware;
using Orbit.Application.Api.Services;

namespace Orbit.Application.Api
{
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
        
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Multitenancy POC", Version = "v1" });  
                c.AddSecurityDefinition("basic", new OpenApiSecurityScheme  
                {  
                    Name = "Authorization",  
                    Type = SecuritySchemeType.Http,  
                    Scheme = "basic",  
                    In = ParameterLocation.Header,
                    Description = "Basic Authorization header using the Bearer scheme."  
                });  
                c.AddSecurityRequirement(new OpenApiSecurityRequirement  
                {  
                    {  
                        new OpenApiSecurityScheme  
                        {  
                            Reference = new OpenApiReference  
                            {  
                                Type = ReferenceType.SecurityScheme,  
                                Id = "basic"  
                            }  
                        },  
                        new string[] {}  
                    }  
                });
            });
        
            services.AddAuthentication("BasicAuthentication")  
                .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);  
        
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapperProfile());
            });
            var mapper = config.CreateMapper();
            services.AddSingleton(mapper);
        
            // Register DI services
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IDomainContextInfo, DomainContextInfo>();
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
            app.UseMultiTenant();
            app.UseStaticFiles();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}