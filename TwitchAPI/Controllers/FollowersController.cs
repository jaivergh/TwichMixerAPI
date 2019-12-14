using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TwitchAPI.Services;

namespace TwitchAPI.Controllers
{
    public class FollowersController : Controller
    {
		public FollowersController(TwitchService twitch)
		{
			this.TwitchService = twitch;
		}

		public TwitchService TwitchService { get; }

		public int Index()
        {
			return TwitchService.CurrentFollowerCount; ;
        }
    }
}