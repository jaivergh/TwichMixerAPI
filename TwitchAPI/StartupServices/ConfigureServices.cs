using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TwitchAPI.Hubs;
using TwitchAPI.Models;
using TwitchAPI.Services;

namespace TwitchAPI.StartupServices
{
	public static class ConfigureServices
	{
		public static void Execute(IServiceCollection services, IConfiguration Configuration)
		{
			services.Configure<CookiePolicyOptions>(options =>
			{
				// This lambda determines whether user consent for non-essential cookies is needed for a given request.
				options.CheckConsentNeeded = context => true;
				options.MinimumSameSitePolicy = SameSiteMode.None;
			});

			services.Configure<FollowerGoalConfiguration>(Configuration.GetSection("FollowerGoal"));
			ConfigureStreamingServices(services, Configuration);
			ConfigureApsNetFeatures(services);
		}

		private static void ConfigureApsNetFeatures(IServiceCollection services)
		{
			services.AddSignalR();
			services.AddSingleton<FollowerHub>();

			services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
		}

		public static void ConfigureStreamingServices(IServiceCollection services, IConfiguration Configuration)
		{
			var sp = services.BuildServiceProvider();
			var svc = new Services.TwitchService(Configuration, sp.GetService<ILoggerFactory>());
			services.AddSingleton<IHostedService>(svc);
			services.AddSingleton(svc);

			var mrx = new Services.MixerService(Configuration, sp.GetService<ILoggerFactory>());
			services.AddSingleton<IHostedService>(mrx);
			services.AddSingleton(mrx);

			services.AddSingleton<MyFollowerService>();
		}
	}
}
