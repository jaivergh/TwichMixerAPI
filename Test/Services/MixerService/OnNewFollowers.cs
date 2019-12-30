using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Test.Services.MixerService
{
	public class OnNewFollowers : BaseFixture
	{
		public OnNewFollowers()
		{
			CreateLogger();
			this.MockConfiguration = Mockery.Create<IConfiguration>();
		}

		private void CreateLogger()
		{
			MockLoggerFactory = Mockery.Create<ILoggerFactory>();
			MockLogger = Mockery.Create<ILogger>();

			MockLoggerFactory.Setup(f => f.CreateLogger(It.IsAny<string>())).Returns(MockLogger.Object);
		}

		private Mock<ILoggerFactory> MockLoggerFactory { get; set; }
		private Mock<ILogger> MockLogger { get; set; }
		private Mock<IConfiguration> MockConfiguration { get; set; }

		[Fact]
		public void ShouldSetCurrentFollowerCount()
		{
			// arrange
			var newFollowerCount = new Random().Next(400, 500);
			//act
			var sut = new TwitchAPI.Services.MixerService(MockConfiguration.Object, MockLoggerFactory.Object);

			//assert
		}
	}
}
