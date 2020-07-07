namespace ScoreManager {
	[System.Serializable]
	public class Config {
    
		public bool shouldSaveInSongDir = true;
		public string globalScoreSavePath;
		public bool showOverlay = true;
		public bool showMultiplier = true;
		public bool showOverallScore = true;
		public bool showCurrentTargetScore = true;
	}
}