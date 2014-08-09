namespace BowlingScore.Domain.Entities
{
	/* Entity representing a specific role. Should be read-only from the consumer's point of view.
	 * The repository will create this and return a domain object.
	 */
	public class Roll
	{
		public int Id { get; set; }
	
		public int PlayerId { get; set; }
	
		public int GameId { get; set; }
	
		public int PinsKnockedDown { get; set; }
		
		public int Frame { get; set; }
	
		public int RollInFrame { get; set; }
	}
}