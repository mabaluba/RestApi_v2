using BusinessLogic;
using BusinessLogic.NotivicationServices;
using DataAccess;
using FluentValidation.AspNetCore;
using M10_RestApi.ExceptionMiddleware;
using M10_RestApi.ModelsDto;
using M10_RestApi.ModelsValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace M10_RestApi
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
            services.AddControllers().AddXmlSerializerFormatters();

            services.AddFluentValidation();

            services.AddRestApiValidationServices();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "M10_RestApi", Version = "v1" });
            });

            services
                .AddBusinessLogic();

            services

                // .AddDataAccessServices(Configuration.GetConnectionString("MSSqlEducationDb"));
                .AddDataAccessServices(Configuration.GetConnectionString("PostgreEducationDb"));

            services.AddAutoMapper(typeof(RestApiMapperProfile));

            services.Configure<EducationMailContacts>(Configuration.GetSection("EducationMailContacts"));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "M10_RestApi v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseStatusCodePages();

            app.UseMiddleware<AppExceptionHandlerMiddleware>();

            app.UseEndpoints(endMark =>
            {
                endMark.MapControllers();
            });
        }
    }
}