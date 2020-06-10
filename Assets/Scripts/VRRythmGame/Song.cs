namespace VRRythmGame {
	[System.Serializable]
	public class Song {
		public int id;
		public string formatVersion;
		public string songName;
		public string songSubName;
		public string songAuthorName;
		public string albumName;
		public string levelAuthorName;
		public float beatsPerMinute;
		public float previewStartTime;
		public string songFile = "song.eeg";
		public string coverImageFile = "cover.jpg";
		public float startTimeOffset;
		public TrackingPoint[] trackingPoints;
		public string leftHandTool;
		public string rightHandTool;
		public string waistTool;
		public string leftFootTool;
		public string rightFootTool;
		public string environment;
		public string targetObject = "cube";
		public string obstacleObject;
		public Difficulty[] difficulties;

	}
}
