using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using TwitchAPI.Models;
using TwitchAPI.Services;

namespace TwitchAPI.Controllers
{
	public class FollowersController : Controller
	{
		private static int _TestFollowers;

		public FollowersController(
			TwitchService twitch, 
			MixerService mixer, 
			IOptions<FollowerGoalConfiguration> config,
			IHostingEnvironment env
			)
		{
			this.TwitchService = twitch;
			MixerService = mixer;
			this.Configuration = config.Value;
			this.HostingEnvironment = env;
		}

		public TwitchService TwitchService { get; }
		public MixerService MixerService { get; }
		public FollowerGoalConfiguration Configuration { get; private set; }
		public IHostingEnvironment HostingEnvironment { get; }

		[HttpGet("api/Followers")]
		public int Get()
		{
			if (HostingEnvironment.IsDevelopment() && _TestFollowers > 0)
			{
				return _TestFollowers;
			}
			//return TwitchService.CurrentFollowerCount; ;
			return MixerService.CurrentFollowerCount;
		}

		[HttpPost("api/Followers")]
		public void Post(int newFollowers)
		{
			if (HostingEnvironment.IsDevelopment())
			{
				_TestFollowers = newFollowers;
			}
		}

		public IActionResult Count()
		{
			return View(MixerService.CurrentFollowerCount);
		}

		[Route("followers/goal/{*stuff}")]
		public IActionResult Goal(string stuff)
		{
			return View("Docs_Goal");
		}

		[Route("followers/goal/{goal:int}/{caption:maxlength(25)}")]
		public IActionResult Goal(string caption = "", int goal = 0, int width = 800)
		{
			caption = string.IsNullOrEmpty(caption) ? Configuration.Caption : caption;
			goal = goal == 0 ? Configuration.Goal : goal;

			ViewBag.Width = width;

			return View(new FollowerGoal
			{
				Caption = caption,
				CurrentValue = MixerService.CurrentFollowerCount,
				GoalValue = goal
			});
		}

		[Route("followers/goal/preview")]
		public IActionResult PreviewGoal()
		{
			return View();
		}
	}
}