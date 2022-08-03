using AzureBlobStorage.WebApi.Repository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Builder;
using Autofac.Extensions.DependencyInjection;

namespace AzureBlobStorage.WebApi
{
	public class Program
	{
		public static void Main(string[] args)
		{
			CreateHostBuilder(args).Build().Run();
		}

		/// <summary>
		/// Setup a Host Builder
		/// </summary>
		/// <param name="args">argument variables passed from outside the commandline</param>
		/// <returns>The HostBuilder</returns>
		internal static IHostBuilder CreateHostBuilder(string[] args)
		{

			return Host.CreateDefaultBuilder(args)
					//Register Autofac as a Service Provicer for DI
					.UseServiceProviderFactory(new AutofacServiceProviderFactory())
					//Set Default statrup code
					.ConfigureWebHostDefaults(webBuilder =>
					{
						webBuilder.UseStartup<Startup>();
					})
					//Configure NLog as the default Logger
					.ConfigureLogging(logging =>
					{
						//Remove default log providers
						logging.ClearProviders();
						//Set minimum Level of logging as Tracing
						logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
					});
		}
	}
}
