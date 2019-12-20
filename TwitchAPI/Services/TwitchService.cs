using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TwitchLib.Api.Models.Helix.Users.GetUsersFollows;
using TwitchLib.Api.Models.v5.Users;
using TwitchLib.Api.Services;
using TwitchLib.PubSub;

namespace TwitchAPI.Services
{
	public class TwitchService : IHostedService
	{
		/// <summary>
		/// Service for connecting and monitoring Twitch
		/// </summary>
		public FollowerService Service { get; private set; }
		public IConfiguration Configuration { get; }

		public TwitchService(IConfiguration config)
		{
			this.Configuration = config;
		}

		public Task StartAsync(CancellationToken cancellationToken)
		{
			return StartTwitchMonitoring();
		}
		
		public Task StopAsync(CancellationToken cancellationToken)
		{
			return StopTwitchMonitoring();
		}

		public static int _CurrentFollowerCount;
		public int CurrentFollowerCount { get { return _CurrentFollowerCount; } }

		private async Task StartTwitchMonitoring()
		{
			return;
			string clientId = Configuration["StreamServices:Twitch:ClientId"];
			var twitchApi = new TwitchLib.Api.TwitchAPI();
			try
			{
				await twitchApi.Settings.SetClientIdAsync(clientId);
			}
			catch (Exception e)
			{

				throw e;
			}

			try
			{
				var userFollows = await twitchApi.Users.v5.GetUserByNameAsync("jcamilogh");
			}
			catch (Exception e)
			{

				throw e;
			} //.Helix.Users.GetUsersFollowsAsync("user_id");


			Service = new FollowerService(twitchApi);
			LiveStreamMonitor lms = new LiveStreamMonitor(twitchApi);

			lms.SetStreamsByUsername(new List<string>() { "jcamilogh" });
			Service.SetChannelByName(Configuration["StreamServices:Twitch:Channel"]);

			try
			{
				lms.StartService();
				await Service.StartService();
			}
			catch (Exception e)
			{

				throw e;
			}

			var follows = twitchApi.Channels.v5.GetAllFollowersAsync(Configuration["StreamServices:Twitch:UserId"]);
			_CurrentFollowerCount = follows.Result.Count;
			lms.OnStreamOnline += Lms_OnStreamOnline;

			_CurrentFollowerCount = Service.QueryCount;

			Service.OnNewFollowersDetected += Service_OnNewFollowersDetected;
		}

		private void Lms_OnStreamOnline(object sender, TwitchLib.Api.Services.Events.LiveStreamMonitor.OnStreamOnlineArgs e)
		{
			Interlocked.Exchange(ref _CurrentFollowerCount, e.Stream.Viewers);
		}

		private void Service_OnNewFollowersDetected(object sender, TwitchLib.Api.Services.Events.FollowerService.OnNewFollowersDetectedArgs e)
		{
			Interlocked.Exchange(ref _CurrentFollowerCount, e.QueryCount);
		}

		private Task StopTwitchMonitoring()
		{
			Service.StopService();
			return Task.CompletedTask;
		}
	}
}
