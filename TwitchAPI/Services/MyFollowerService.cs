using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TwitchAPI.Hubs;

namespace TwitchAPI.Services
{
	public class MyFollowerService
	{
		public MyFollowerService(TwitchService twitch,
			MixerService mixer,
			IConfiguration config,
			IHubContext<FollowerHub> followerHubContext)
		{
			this.Twitch = twitch;
			this.Mixer = mixer;
			this.Config = config;
			this.FollowerContext = followerHubContext;
			Mixer.Updated += Mixer_Updated;
		}

		private void Mixer_Updated(object sender, EventArgs e)
		{
			throw new NotImplementedException();
		}

		public TwitchService Twitch { get; }
		public MixerService Mixer { get; }
		public IConfiguration Config { get; }
		public IHubContext<FollowerHub> FollowerContext { get; }

		private int TotalFollowerCount
		{
			get
			{
				return Mixer.CurrentFollowerCount;
			}
		}

		public void FollowersUpdated()
		{
			FollowerContext.Clients.All.SendAsync("OnFollowersCountUpdated", TotalFollowerCount);
		}
	}
}
