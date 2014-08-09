namespace BowlingScore.Domain
{
	using System.Collections.Generic;
	using BowlingScore.Domain.Entities;

	
	public interface IRollRepository
	{
		// Insert
		Roll Save(int playerId, int gameId, int pins);
		
		// Insert or Update
		/* TODO: Is this something we want? We should look at our use cases and decide if
		 * we want a more general "Save" function or more specific updates
		 */
		Roll Save(Roll roll);
		
		/* Constraints: This comes back sorted by Frame, then by RollInFrame */
		List<Roll> GetAll(int playerId, int gameId);
	}
}