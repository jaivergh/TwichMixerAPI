using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Mixer.Base;
using Mixer.Base.Model.Channel;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TwitchAPI.Services
{
	public class MixerService : IHostedService
	{
		private IConfiguration Configuration;

		private static readonly List<OAuthClientScopeEnum> _scopes = new List<OAuthClientScopeEnum>()
		{
			OAuthClientScopeEnum.channel__details__self,
			OAuthClientScopeEnum.channel__update__self

		};

		public MixerService(IConfiguration configuration)
		{
			this.Configuration = configuration;
		}

		private ExpandedChannelModel _MyChannel;
		private static int _CurrentFollowerCount;
		private Timer _Timer;

		public int CurrentFollowerCount { get { return _CurrentFollowerCount; } }

		public async Task StartAsync(CancellationToken cancellationToken)
		{
			var clientId = Configuration["StreamServices:Mixer:ClientId"];
			MixerConnection connection;
			try
			{
				//string ghd = await MixerConnection.GetAuthorizationCodeURLForOAuthBrowser(clientId, _scopes, "localhost");
				connection = await MixerConnection.ConnectViaLocalhostOAuthBrowser(clientId, _scopes);
			}
			catch (Exception e)
			{

				throw e;
			}
			_MyChannel = await connection.Channels.GetChannel(Configuration["StreamServices:Mixer:Channel"]);
			_CurrentFollowerCount = (int)_MyChannel.numFollowers;

			_Timer = new Timer(NewFollowerCheck, null, 5000, 5000);
		}

		private void NewFollowerCheck(object state)
		{
			_CurrentFollowerCount = Interlocked.Exchange(ref _CurrentFollowerCount, (int)_MyChannel.numFollowers);
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			//throw new NotImplementedException();
			_Timer.Dispose();
			return Task.CompletedTask;
		}
	}
}
