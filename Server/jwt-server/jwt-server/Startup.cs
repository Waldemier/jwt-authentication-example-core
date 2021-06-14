using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Joonasw.AspNetCore.SecurityHeaders;
using Joonasw.AspNetCore.SecurityHeaders.Csp.Builder;
using jwt_server.Context;
using jwt_server.Helpers;
using jwt_server.Repositories.Implements;
using jwt_server.Repositories.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace jwt_server
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
            services.AddCors();
            
            services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"))
                );
            
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<JwtService>();
            
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "jwt_server", Version = "v1"});
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "jwt_server v1"));
            }
    
            // Additional security headers
            app.Use(async (context, next) =>
            {
                context.Response.Headers.Add("X-Xss-Protection", "1"); // принудительно включить встроенный механизм защиты браузера от XSS атак.
                context.Response.Headers.Add("X-Frame-Options", "SAMEORIGIN"); // для защиты от атак типа clickjacking.
                context.Response.Headers.Add("X-Content-Type-Options", "noshiff"); // для защиты от подмены MIME типов.
                context.Response.Headers.Add("X-Powered-By", "ASP.NET");
                await next();
            });
            
            app.UseHttpsRedirection();

            app.UseRouting();

            // do not sure how it work (additional security) (content-security-policy)
            app.UseCsp(opt =>
            {
                opt.AllowStyles
                    .FromSelf();
                    //.From("any site");
            });
            
            app.UseCors(options =>
            {
                options.WithOrigins(new[] {"http://localhost:3000"});
                options.AllowAnyHeader();
                options.AllowAnyMethod();
                options.AllowCredentials(); // Allows us to send a cookie to the frontend 
            });
            
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}