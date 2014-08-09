namespace BowlingCalculator.Tests.Domain.UnitTests
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using BowlingScore.Domain;
	using BowlingScore.Domain.Engine;
	using BowlingScore.Domain.Entities;
	using NUnit.Framework;

	public class TestRollRepository : IRollRepository
	{
		private List<Roll> rolls = new List<Roll>();
		
		private BowlingRules rules;
		
		public TestRollRepository(BowlingRules rules)
		{
			this.rules = rules;
		}
		
		public Roll Save(Roll roll)
		{
			if(roll.Id <= 0)
			{
				roll.Id = this.rolls.Count + 1;
				this.rolls.Add(roll);
				
				return roll;
			}
			else
			{
				Roll rollFromRepo = this.rolls[roll.Id - 1];
				rollFromRepo.Frame = roll.Frame;
				rollFromRepo.GameId = roll.GameId;
				rollFromRepo.PinsKnockedDown = roll.PinsKnockedDown;
				rollFromRepo.PlayerId = roll.PlayerId;
				rollFromRepo.RollInFrame = roll.RollInFrame;
				
				return roll;
			}
		}
		
		public List<Roll> GetAll(int playerId, int gameId)
		{
			return this.rolls.Where(x => x.PlayerId == playerId && x.GameId == gameId).OrderBy(x => x.Frame).ThenBy(x => x.RollInFrame).ToList();
		}
		
		public Roll Save(int playerId, int gameId, int pins)
		{
			Roll roll = new Roll();
			roll.PlayerId = playerId;
			roll.GameId = gameId;
			roll.PinsKnockedDown = pins;
			
			IEnumerable<Roll> rolls = this.rolls.Where(x => x.PlayerId == playerId && x.GameId == gameId);
			if(rolls.Count() == 0)
			{
				roll.RollInFrame = 1;
				roll.Frame = 1;
				return this.Save(roll);
			}
			
			Roll lastRoll = rolls.OrderByDescending(x => x.Frame).ThenByDescending(x => x.RollInFrame).First();
			// Special magic to extend the last frame.
			if(lastRoll.Frame == this.rules.MaxFrames)
			{
				List<Roll> lastFrame = this.GetAll(playerId, gameId).Where(x => x.Frame == lastRoll.Frame).ToList();
				bool allowedExtras = this.rules.ExtraRollsInFrame(lastFrame);
				if(lastFrame.Count < 2 || (allowedExtras && lastFrame.Count < 3))
				{
					roll.RollInFrame = lastRoll.RollInFrame + 1;
					roll.Frame = lastRoll.Frame;
					return this.Save(roll);
				}
				else
				{
					throw new IndexOutOfRangeException();
				}
				
			}
			if(this.rules.IsStrike(lastRoll) || lastRoll.RollInFrame == rules.RollsInFrame)
			{
				roll.RollInFrame = 1;
				roll.Frame = lastRoll.Frame + 1;
				return this.Save(roll);
			}
			
			roll.RollInFrame = lastRoll.RollInFrame + 1;
			roll.Frame = lastRoll.Frame;
			return this.Save(roll);
		}
	}
}