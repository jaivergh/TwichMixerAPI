using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Test
{
	public abstract class BaseFixture
	{
		public BaseFixture()
		{
			Mockery = new MockRepository(MockBehavior.Loose);
		}
		public MockRepository Mockery { get; private set; }
	}

	
}
