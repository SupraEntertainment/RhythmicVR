namespace BeatSaber {
	[System.Serializable]
	public class BSMESong {
		private int _time;
		private int[] _BPMChanges;
		private BSEvent[] _events;
		private BSMENote[] _notes;
		private BSMEObstacle[] _obstacles;
		private string[] _bookmarks;

		public Song ToSong() {
			return new Song();
		}
	}
}
