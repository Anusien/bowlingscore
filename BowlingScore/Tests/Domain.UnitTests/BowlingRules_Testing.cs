namespace BowlingScore.Tests.Domain.UnitTests
{
	using System.Collections.Generic;
	using BowlingScore.Domain.Engine;
	using BowlingScore.Domain.Entities;
	using NUnit;
	using NUnit.Framework;
	
	[TestFixture]
	public class BowlingRules_Testing
	{
		private BowlingRules rules;
		
		private Roll strike;
		
		private List<Roll> spare;
		
		private List<Roll> openFrame;
		
		private Roll gutterBall;
		
		[SetUp]
		public void Arrange()
		{
			this.rules = new BowlingRules(10, 2, 10);
			
			this.strike = new Roll
			{
				PinsKnockedDown = 10,
				RollInFrame = 1
			};
			
			Roll spare1 = new Roll
			{
				PinsKnockedDown = 5,
				RollInFrame = 1
			};
			Roll spare2 = new Roll
			{
				PinsKnockedDown = 5,
				RollInFrame = 2
			};
			
			this.spare = new List<Roll>
			{
				spare1,
				spare2
			};
			
			Roll open1 = new Roll
			{
				PinsKnockedDown = 7,
				RollInFrame = 1
			};
			Roll open2 = new Roll
			{
				PinsKnockedDown = 2,
				RollInFrame = 1
			};
			
			this.openFrame = new List<Roll>
			{
				open1,
				open2
			};
			
			
			this.gutterBall = new Roll
			{
				PinsKnockedDown = 0,
				RollInFrame = 1
			};
		}
		
		[Test]
		public void ShouldDetectStrike()
		{
			List<Roll> frame = new List<Roll>();
			
			frame.Add(this.strike);
			
			Assert.True(this.rules.IsStrike(this.strike));
			Assert.True(this.rules.IsStrike(frame));
		}
		
		[Test]
		public void ShouldNotDetectSpareWhenStrike()
		{
			List<Roll> frame = new List<Roll>();
			frame.Add(this.strike);
			Assert.False(this.rules.IsSpare(frame));
		}
		
		[Test]
		public void ShouldDetectSpare()
		{
			Assert.True(this.rules.IsSpare(this.spare));
		}
		
		[Test]
		public void ShouldNotDetectStrikeWhenSpare()
		{
			Assert.False(this.rules.IsStrike(this.spare));
		}
		
		[Test]
		public void ShouldProperlyHandleOpenFrame()
		{
			Assert.False(this.rules.IsStrike(this.openFrame));
			Assert.False(this.rules.IsSpare(this.openFrame));
		}
	}
}