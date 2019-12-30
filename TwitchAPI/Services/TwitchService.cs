using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
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

		public TwitchService(IConfiguration config, ILoggerFactory loggerFactory)
		{
			this.Configuration = config;
			ClientId = Configuration["StreamServices:Twitch:ClientId"];
			this.Logger = loggerFactory.CreateLogger("StreamServices");
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

		public static int _CurrentViewerCount = 10;
		public int CurrentViewCount { get { return _CurrentViewerCount; } }

		public int _CurrentViewersCount { get; private set; }
		public Timer _Timer { get; private set; }
		public string ClientId { get; private set; }
		public ILogger Logger { get; }

		private async Task StartTwitchMonitoring()
		{
			return;
			Users userFollows;
			
			var twitchApi = new TwitchLib.Api.TwitchAPI();
			try
			{
				await twitchApi.Settings.SetClientIdAsync(ClientId);
			}
			catch (Exception e)
			{

				throw e;
			}

			try
			{
				userFollows = await twitchApi.Users.v5.GetUserByNameAsync("jcamilogh");
			}
			catch (Exception e)
			{

				throw e;
			} //.Helix.Users.GetUsersFollowsAsync("user_id");

			_CurrentFollowerCount = userFollows.Total;
			Service = new FollowerService(twitchApi);
			LiveStreamMonitor lms = new LiveStreamMonitor(twitchApi);

			lms.SetStreamsByUsername(new List<string>() { "jcamilogh" });
			Service.SetChannelByName(Configuration["StreamServices:Twitch:Channel"]);

			//try
			//{
			//	lms.StartService();
			//	await Service.StartService();
			//}
			//catch (Exception e)
			//{

			//	throw e;
			//}

			//var follows = twitchApi.Channels.v5.GetAllFollowersAsync(Configuration["StreamServices:Twitch:UserId"]);
			//_CurrentFollowerCount = follows.Result.Count;
			//lms.OnStreamOnline += Lms_OnStreamOnline;

			//_CurrentFollowerCount = Service.QueryCount;

			//Service.OnNewFollowersDetected += Service_OnNewFollowersDetected;

			var V5Stream = new TwitchLib.Api.Sections.Streams.V5(api: twitchApi);
			var stream = await V5Stream.GetStreamByUserAsync(Configuration["StreamServices:Twitch:Channel"]);
			_CurrentViewersCount = stream.Stream.Viewers;

			_Timer = new Timer(CheckViews, null, 0, 5000);
		}

		private async void CheckViews(object state)
		{
			var twitchApi = new TwitchLib.Api.TwitchAPI();
			await twitchApi.Settings.SetClientIdAsync(ClientId);
			var V5Stream = new TwitchLib.Api.Sections.Streams.V5(api: twitchApi);
			var stream = await V5Stream.GetStreamByUserAsync(Configuration["StreamServices:Twitch:Channel"]);
			_CurrentViewersCount = stream.Stream.Viewers;

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
