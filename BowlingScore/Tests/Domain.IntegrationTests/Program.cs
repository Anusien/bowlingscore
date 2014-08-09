namespace BowlingScore.Tests.Domain.IntegrationTests
{
	using System;
	
	using BowlingScore.Domain;
	using BowlingScore.Domain.Engine;
	using BowlingCalculator.Tests.Domain.UnitTests;
	
	public class Program
	{
		public static void Main(string[] args)
		{
			BowlingRules rules = new BowlingRules(10, 2, 10);
			TestRollRepository repo = new TestRollRepository(rules);
			ScoreCalculator calculator = new ScoreCalculator(repo, rules);
			int playerId = 1;
			int gameId = 1;
			
			Console.WriteLine("Enter any invalid input to exit.");
			
			while(true)
			{
				
				Console.Write("Pins knocked down: ");
				string stringpins = Console.ReadLine();
				
				int pins;
				
				if(!int.TryParse(stringpins, out pins) || pins < 0 || pins > 10)
				{
					return;
				}
				try
				{
					repo.Save(playerId, gameId, pins);
					Console.WriteLine(calculator.Calculate(playerId, gameId));
				}
				catch(IndexOutOfRangeException)
				{
					Console.WriteLine("That game is over already.");
				}
			}
		}
	}
}
