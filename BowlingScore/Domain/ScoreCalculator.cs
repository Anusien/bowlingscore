namespace BowlingScore.Domain
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using BowlingScore.Domain.Engine;
	using BowlingScore.Domain.Entities;

	public class ScoreCalculator {
		
		private IRollRepository rollRepository;
		private BowlingRules rules;
		
		public ScoreCalculator(IRollRepository rollRepository, BowlingRules rules)
		{
			this.rollRepository = rollRepository;
			this.rules = rules;
		}
		
		// Get the score for a specific player and id.
		public int Calculate(int playerId, int gameId)
		{
			List<Roll> rolls = this.rollRepository.GetAll(playerId, gameId);
			IEnumerable<List<Roll>> remainingFrames = GetFrames(rolls);
			
			int score = 0;
			
			if(!remainingFrames.Any())
			{
				throw new KeyNotFoundException();
			}
			
			while(remainingFrames.Any())
			{
				List<Roll> frame = remainingFrames.First();
				remainingFrames = remainingFrames.Skip(1);
				
				score += CalculateFrame(frame, remainingFrames);
			}
			
			return score;
		}
		
		private int CalculateFrame(List<Roll> frame, IEnumerable<List<Roll>> remainingFrames)
		{
			int score = SumFrame(frame);
			if(this.rules.IsStrike(frame))
			{
				return score + GetNextNScores(remainingFrames, 2);
			}
			else if(this.rules.IsSpare(frame))
			{
				return score + GetNextNScores(remainingFrames, 1);
			}
			else
			{
				return SumFrame(frame);
			}
		}
		
		private static int GetNextNScores(IEnumerable<List<Roll>> frames, int howMany)
		{
			int sum = 0;
			IEnumerable<List<Roll>> remainingFrames = frames;
			IEnumerable<Roll> rollsLeftInFrame = frames.FirstOrDefault();
			
			if(howMany < 0)
			{
				throw new IndexOutOfRangeException();
			}
			if(howMany == 0 || rollsLeftInFrame == null)
			{
				return 0;
			}
			for(int i = 0; i < howMany; i++)
			{
				if(!rollsLeftInFrame.Any())
				{
					remainingFrames = remainingFrames.Skip(1);
					rollsLeftInFrame = remainingFrames.FirstOrDefault();
					if(rollsLeftInFrame == null)
					{
						break;
					}
				}
				if(rollsLeftInFrame.Any())
				{
					sum += rollsLeftInFrame.First().PinsKnockedDown;
					rollsLeftInFrame = rollsLeftInFrame.Skip(1);
				}
			}
			return sum;
		}
		
		private static int SumFrame(List<Roll> frame)
		{
			return frame.Sum(roll => roll.PinsKnockedDown);
		}
		
		private static List<List<Roll>> GetFrames(List<Roll> rolls)
		{
			List<List<Roll>> frames = new List<List<Roll>>();
			int frameNumber = -1;
			List<Roll> frame = null;
			
			/* We require that the repository return these in order, so no need to sort here */
			foreach(Roll roll in rolls)
			{
				if(roll.Frame > frameNumber)
				{
					frameNumber = roll.Frame;
					
					frame = new List<Roll>();
					frame.Add(roll);
					
					frames.Add(frame);
				}
				else
				{
					frame.Add(roll);
				}
			}
			
			return frames;
		}
	}
}