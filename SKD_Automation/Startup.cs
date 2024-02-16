using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AM.Persistence;
using Microsoft.EntityFrameworkCore;
using FluentValidation.AspNetCore;

using AM.Domain.Validator;
using System.Reflection;
using FluentValidation;
using AM.Domain.Entities;
using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using SKD_Automation.Middlewares;

namespace SKD_Automation
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
            services.AddControllers().AddFluentValidation();
            services.AddFluentValidationValidators();

            services.AddDbContext<AutomationDbService>(option => option.UseSqlServer(Configuration.GetConnectionString("automation")));
            services.AddScoped<IUnitWorkService, UnitWorkService>();
            services.AddAutoMapper(typeof(Program).Assembly);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Automation", Version = "v1" });
            });
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //This below is the custom middleware for handling exception globally for dev and production
            app.UseMiddleware<ExceptionMiddleware>();

            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "SKD_Automation v1"));
            }


            app.UseRouting();


            //It will allow diffent domain can access 
            app.UseCors(m => m.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }


    public static class AddFluentValidationExtension
    {
        public static void AddFluentValidationValidators(this IServiceCollection service)
        {
            service.AddTransient<IValidator<Department>, DepartmentValidator>();
            service.AddTransient<IValidator<Plugin>, PluginValidator>();
            service.AddTransient<IValidator<PluginLog>, PluginLogValidator>();
        }
    }
}
