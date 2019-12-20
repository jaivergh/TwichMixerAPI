using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using TwitchAPI.Models;
using TwitchAPI.Services;

namespace TwitchAPI.Controllers
{
	public class FollowersController : Controller
	{
		public FollowersController(TwitchService twitch, MixerService mixer, IOptions<FollowerGoalConfiguration> config)
		{
			this.TwitchService = twitch;
			MixerService = mixer;
			this.Configuration = config.Value;
		}

		public TwitchService TwitchService { get; }
		public MixerService MixerService { get; }
		public FollowerGoalConfiguration Configuration { get; private set; }

		[HttpGet("api/Followers")]
		public int Get()
		{

			//return TwitchService.CurrentFollowerCount; ;
			return MixerService.CurrentFollowerCount;
		}

		public IActionResult Count()
		{
			return View(MixerService.CurrentFollowerCount);
		}

		[Route("followers/goal/{goal=0}/{caption=}")]
		public IActionResult Goal(string caption, int goal, int width = 800)
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
	}
}