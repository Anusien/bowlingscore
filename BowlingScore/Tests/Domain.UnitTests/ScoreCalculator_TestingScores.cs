using System.Collections.Generic;
namespace BowlingScore.Tests.Domain.UnitTests
{
	using BowlingScore.Domain;
	using BowlingScore.Domain.Engine;
	using BowlingCalculator.Tests.Domain.UnitTests;
	using NUnit.Framework;

	[TestFixture]
	public class ScoreCalculator_TestingScores
	{
		private TestRollRepository repository;
		private BowlingRules rules;
		private const int Player1 = 1, Player2 = 2, UnknownPlayer = 3;
		private const int Game1 = 1, Game2 = 2, UnknownGame = 3;
		
		[SetUp]
		public void Arrange()
		{
			this.rules = new BowlingRules(10, 2, 10);
			this.repository = new TestRollRepository(rules);
		}
		
		[Test]
		public void Test_Spare()
		{
			this.repository.Save(Player1, Game1, 1);
			this.repository.Save(Player1, Game1, 9);
			this.repository.Save(Player1, Game1, 1);
			this.repository.Save(Player1, Game1, 2);
			
			ScoreCalculator calculator = new ScoreCalculator(this.repository, this.rules);
			Assert.AreEqual(14, calculator.Calculate(Player1, Game1));
		}
		
		[Test]
		public void Test_Invalid_Player()
		{
			this.repository.Save(Player1, Game1, 1);
			ScoreCalculator calculator = new ScoreCalculator(this.repository, this.rules);
			Assert.Throws<KeyNotFoundException>(() => calculator.Calculate(UnknownPlayer, Game1));
		}
		
		[Test]
		public void Test_Invalid_Game()
		{
			this.repository.Save(Player1, Game1, 1);
			ScoreCalculator calculator = new ScoreCalculator(this.repository, this.rules);
			Assert.Throws<KeyNotFoundException>(() => calculator.Calculate(Player1, UnknownGame));
		}
		
		[Test]
		public void Test_Strike()
		{
			this.repository.Save(Player1, Game1, 10);
			this.repository.Save(Player1, Game1, 5);
			this.repository.Save(Player1, Game1, 1);
			this.repository.Save(Player1, Game1, 3);
			
			ScoreCalculator calculator = new ScoreCalculator(this.repository, this.rules);
			Assert.AreEqual(25, calculator.Calculate(Player1, Game1));
		}
		
		[Test]
		public void Test_StrikeThroughTwoFrames()
		{
			this.repository.Save(Player1, Game1, 10);
			this.repository.Save(Player1, Game1, 10);
			this.repository.Save(Player1, Game1, 10);
			
			ScoreCalculator calculator = new ScoreCalculator(this.repository, this.rules);
			Assert.AreEqual(60, calculator.Calculate(Player1, Game1));
		}
		
		[Test]
		public void Games_Are_Separate()
		{
			this.repository.Save(Player1, Game1, 10);
			this.repository.Save(Player1, Game2, 5);
			
			ScoreCalculator calculator = new ScoreCalculator(this.repository, this.rules);
			Assert.AreEqual(10, calculator.Calculate(Player1, Game1));
			Assert.AreEqual(5, calculator.Calculate(Player1, Game2));
		}
		
		[Test]
		public void Players_Are_Separate()
		{
			this.repository.Save(Player1, Game1, 10);
			this.repository.Save(Player2, Game1, 5);
			
			ScoreCalculator calculator = new ScoreCalculator(this.repository, this.rules);
			Assert.AreEqual(10, calculator.Calculate(Player1, Game1));
			Assert.AreEqual(5, calculator.Calculate(Player2, Game1));
		}
		
		[Test]
		public void Test_Strike_At_Game_End()
		{
			// Fill frames 1-9 with rolls
			for(int i = 1; i < 10; i++)
			{
				this.repository.Save(Player1, Game1, i);
				this.repository.Save(Player1, Game1, 0);
			}
			
			this.repository.Save(Player1, Game1, 10); // Strike
			this.repository.Save(Player1, Game1, 10); // Extra roll 1
			this.repository.Save(Player1, Game1, 10); // Extra roll 2
			
			ScoreCalculator calculator = new ScoreCalculator(this.repository, this.rules);
			Assert.AreEqual(1+2+3+4+5+6+7+8+9+10+10+10, calculator.Calculate(Player1, Game1));
		}
		
		[Test]
		public void Test_Spare_At_Game_End()
		{
			// Fill frames 1-9 with rolls
			for(int i = 1; i < 10; i++)
			{
				this.repository.Save(Player1, Game1, i);
				this.repository.Save(Player1, Game1, 0);
			}
			
			this.repository.Save(Player1, Game1, 5); // Roll
			this.repository.Save(Player1, Game1, 5); // Spare
			this.repository.Save(Player1, Game1, 10); // Extra roll 1
			
			ScoreCalculator calculator = new ScoreCalculator(this.repository, this.rules);
			Assert.AreEqual(1+2+3+4+5+6+7+8+9+5+5+10, calculator.Calculate(Player1, Game1));
		}
		
		[Test]
		public void Test_Perfect_Game()
		{
			for(int i = 1; i <= 12; i++)
			{
				this.repository.Save(Player1, Game1, 10);
			}
			
			ScoreCalculator calculator = new ScoreCalculator(this.repository, this.rules);
			Assert.AreEqual(300, calculator.Calculate(Player1, Game1));
		}
	}
}