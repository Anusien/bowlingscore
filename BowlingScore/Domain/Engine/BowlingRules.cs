namespace BowlingScore.Domain.Engine
{
	using System.Collections.Generic;
	using System.Linq;
	
	using BowlingScore.Domain.Entities;

	/* Holds information about the specific constraints of this Bowling Game.
	 * Allows us to be more flexible with how many pins in a frame, how many frames in a game,
	 * and other things like that.
	 */
	public class BowlingRules
	{
		public int MaxPins { get; private set; }
		
		public int MaxFrames { get; private set; }
		
		public int RollsInFrame { get; private set; }
		
		public BowlingRules(int maxPins, int rollsInFrame, int maxFrames)
		{
			this.MaxPins = maxPins;
			this.RollsInFrame = rollsInFrame;
			this.MaxFrames = maxFrames;
		}
		
		public bool IsSpare(IEnumerable<Roll> frame)
		{
			if(frame.Count() == 2 && SumFrame(frame) == this.MaxPins)
			{
				return true;
			}
			
			return false;
		}
		
		public bool IsStrike(IEnumerable<Roll> frame)
		{
			if(frame.Count() == 1 && frame.First().PinsKnockedDown == this.MaxPins)
			{
				return true;
			}
			return false;
		}
		
		public bool IsStrike(Roll roll)
		{
			if(roll.RollInFrame == 1 && roll.PinsKnockedDown == this.MaxPins)
			{
				return true;
			}
			return false;
		}
		
		public bool ExtraRollsInFrame(IEnumerable<Roll> frame)
		{
			if(!frame.Any())
			{
				return false;
			}
			
			if(this.IsStrike(frame.First()) || this.IsSpare(frame.Take(2)))
			{
				return true;
			}
			
			return false;
		}
		
		private static int SumFrame(IEnumerable<Roll> frame)
		{
			return frame.Sum(roll => roll.PinsKnockedDown);
		}
	}
}