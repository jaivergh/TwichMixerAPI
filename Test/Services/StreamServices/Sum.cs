using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using TwitchAPI.Services;

namespace Test.Services.StreamServices
{
	public class Sum : BaseFixture
	{


		public void CurrentFollowerCount()
		{
			//arrange
			var mixerStream = Mockery.Create<TwitchAPI.Services.MixerService>();
			var currentFollowers = new Random().Next(10, 100);
			mixerStream.SetupGet(j => j.CurrentFollowerCount).Returns(currentFollowers);

			//act
			//var sut = new MixerService(new[] { mixerStream.Object });

			//assert
		}
	}
}
