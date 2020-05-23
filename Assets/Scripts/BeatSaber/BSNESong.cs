namespace BeatSaber {
	[System.Serializable]
	public class BSNESong {
		private int _time;
		private int[] _BPMChanges;
		private BSEvent[] _events;
		private BSNote[] _notes;
		private BSObstacle[] _obstacles;
		private string[] _bookmarks;

		public Song ToSong() {
			return new Song();
		}
	}
}
