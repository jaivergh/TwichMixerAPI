using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TwitchAPI.Services;

namespace TwitchAPI.Hubs
{
	public class FollowerHub : Hub
	{
		public FollowerHub(MyFollowerService myFollowerService)
		{
			this.MyFollowerService = myFollowerService;
		}

		public MyFollowerService MyFollowerService { get; }

		public void StreamFollowers()
		{
			Clients.All.SendAsync("OnFollowersCountUpdated", MyFollowerService.Mixer.CurrentFollowerCount);// MyFollowerService.Mixer.CurrentFollowerCount;
		}
	}
}
