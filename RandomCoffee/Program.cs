using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RandomCoffee.Database;
using RandomCoffee.Services;
using RandomCoffee.Storers;
using Serilog;

namespace RandomCoffee
{
	public static class Program
	{
		public static async Task Main(string[] args)
		{
			InitLogger();
			var host = CreateHostBuilder(args).Build();
			var serviceScopeFactory = host.Services.GetRequiredService<IServiceScopeFactory>();
			using (var serviceScope = serviceScopeFactory.CreateScope())
			{
				var dbContext = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>();
				await dbContext.Database.MigrateAsync();
			}

			try
			{
				await host.RunAsync();
			}
			catch (Exception e)
			{
				Log.Error(e, "Unexpected Error");
			}
		}

		public static IHostBuilder CreateHostBuilder(string[] args)
			=>
				Host.CreateDefaultBuilder()
					.ConfigureAppConfiguration((hostContext, configApp) =>
					{
						configApp.SetBasePath(hostContext.HostingEnvironment.ContentRootPath);
						configApp.AddJsonFile("appsettings.json", true);
						configApp.AddJsonFile(
							$"appsettings.{hostContext.HostingEnvironment.EnvironmentName}.json",
							true);
					})
					.ConfigureServices((hostContext, serviceCollection) =>
						ConfigureServices(hostContext.Configuration, serviceCollection))
					.UseConsoleLifetime();

		private static void ConfigureServices(IConfiguration configuration, IServiceCollection services)
		{
			services.AddDbContextPool<AppDbContext>(options =>
				options.UseNpgsql(configuration.GetConnectionString("DbContext")));

			services.AddSingleton(configuration);
			services.AddScoped<IPersonStorer, PersonStorer>();
			services.AddScoped<IMeetingStorer, MeetingStorer>();
			services.AddScoped<PersonPointsResolver>();
			services.AddScoped<MeetingService>();
			services.AddHostedService<TelegramBot>();
		}

		private static void InitLogger()
			=>
				Log.Logger = new LoggerConfiguration()
					.Enrich.FromLogContext()
					.WriteTo.Console()
					.CreateLogger();
	}
}
