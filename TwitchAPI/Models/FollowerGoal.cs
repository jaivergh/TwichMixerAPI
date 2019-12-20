using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TwitchAPI.Models
{
	public class FollowerGoal
	{
		/// <summary>
		/// Caption of the goal to display
		/// </summary>
		public string Caption { get; set; }

		/// <summary>
		/// The Current value towards the goal
		/// </summary>
		public int CurrentValue { get; set; }

		/// <summary>
		/// The goal number of followers to hit
		/// </summary>
		public int GoalValue { get; set; }
	}
}
