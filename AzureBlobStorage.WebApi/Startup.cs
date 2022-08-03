using Autofac;
using AzureBlobStorage.WebApi.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace AzureBlobStorage.WebApi
{
	public class Startup
	{
		/// <summary>
		/// Configuration defined
		/// </summary>
		public IConfiguration Configuration { get; }

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="configuration"></param>
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		/// <summary>
		/// This method gets called by the runtime. Use this method to add services to the container.
		/// </summary>
		/// <param name="services">The DI service collection</param>
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddControllers().AddJsonOptions(options => { options.JsonSerializerOptions.IgnoreNullValues = true; }); // null values in the API response would be added to the JSON

			// Register the Swagger generator, defining 1 or more Swagger documents
			services.AddSwaggerGen(opts =>
			{
				opts.SwaggerDoc("v1.0", new OpenApiInfo { Title = "Authentication Microservice", Version = "v1.0" });
				opts.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
				{
					Name = "Authorization",
					Type = SecuritySchemeType.ApiKey,
					Scheme = "Bearer",
					BearerFormat = "JWT",
					In = ParameterLocation.Header,
					Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
				});
				opts.AddSecurityRequirement(new OpenApiSecurityRequirement {
				{
					new OpenApiSecurityScheme {
						Reference = new OpenApiReference {
							Type = ReferenceType.SecurityScheme,
								Id = "Bearer"
						}
					},
					new string[] {}
				}
					});
				////enable xml comments
				//var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
				//var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
				//opts.IncludeXmlComments(xmlPath);

			});

			// TODO: updated allowed origin to make it through config
			// Exposed Headers Content-Disposition was added to be able to retrieve
			// the file name returned in HTTP response headers in the documents controller
			services.AddCors(c =>
			{
				c.AddPolicy("CorsPolicy", options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().WithExposedHeaders("Content-Disposition"));
			});

			services.AddSingleton<IAzureStorage, AzureStorage>();
		}

		/// <summary>
		/// Configure the Autofac Container this is called from Autofac because it is regsitered in Program.cs
		/// Called by the autofac factory .UseServiceProviderFactory(new AutofacServiceProviderFactory())
		/// </summary>
		/// <param name="builder"></param>
		public void ConfigureContainer(ContainerBuilder builder)
		{
		}

		/// <summary>
		/// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		/// </summary>
		/// <param name="app">Application builder</param>
		/// <param name="env">Web host enviroment</param>
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			// Enable middleware to serve generated Swagger as a JSON endpoint.
			app.UseSwagger();

			// Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
			// specifying the Swagger JSON endpoint.
			app.UseSwaggerUI(c =>
			{
				c.SwaggerEndpoint("/swagger/v1/swagger.json", "Customer Documents API");
				c.RoutePrefix = string.Empty;
			});

			app.UseHttpsRedirection();

			app.UseRouting();

			app.UseCors("CorsPolicy");

			app.UseAuthentication();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
