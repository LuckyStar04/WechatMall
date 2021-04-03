using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using SixLabors.ImageSharp.Web.DependencyInjection;
using System;
using WechatMall.Api.Data;
using WechatMall.Api.Services;

namespace WechatMall.Api
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
            services.AddControllers(setup =>
            {
                setup.ReturnHttpNotAcceptable = true;
            })
            .AddNewtonsoftJson(setup =>
            {
                setup.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            }).AddXmlDataContractSerializerFormatters()
            .ConfigureApiBehaviorOptions(setup =>
            {
                setup.InvalidModelStateResponseFactory = context =>
                {
                    var problemDetails = new ValidationProblemDetails(context.ModelState)
                    {
                        Type = "https://tools.ietf.org/html/rfc4918#section-11.2",
                        Title = "Errors occured",
                        Status = StatusCodes.Status422UnprocessableEntity,
                        Detail = "ErrCode 422: Unprocessable Entity",
                        Instance = context.HttpContext.Request.Path
                    };
                    problemDetails.Extensions.Add("traceId", context.HttpContext.TraceIdentifier);
                    return new UnprocessableEntityObjectResult(problemDetails)
                    {
                        ContentTypes = { "application/problem+json" }
                    };
                };
            });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
            });


            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IIndexRepository, IndexRepository>();
            services.AddDbContext<MallDbContext>(option =>
            {
                option.UseMySql(Configuration.GetConnectionString("DefaultConnection"));
            });
            services.AddImageSharp();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WechatMall.Api v1"));
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseImageSharp();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
