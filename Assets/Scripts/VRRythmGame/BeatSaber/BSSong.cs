namespace VRRythmGame.BeatSaber {
	public class BSSong {
		public string _version { get; set; }
		public string _songName { get; set; }
		public string _songSubName { get; set; }
		public string _songAuthorName { get; set; }
		public string _levelAuthorName { get; set; }
		public int _beatsPerMinute { get; set; }
		public int _shuffle { get; set; }
		public double _shufflePeriod { get; set; }
		public double _previewStartTime { get; set; }
		public int _previewDuration { get; set; }
		public string _songFilename { get; set; }
		public string _coverImageFilename { get; set; }
		public string _environmentName { get; set; }
		public int _songTimeOffset { get; set; }
		public CustomData _customData { get; set; }
		public DifficultyBeatmapSet[] _difficultyBeatmapSets { get; set; }

		public Song ToSong() {
			return new Song();
		}
		
	}
	public class CustomData
	{
		public string _customEnvironment { get; set; }
		public string _customEnvironmentHash { get; set; }
		public object[] _contributors { get; set; }
	}

	public class CustomDifficultyData {
		public string _difficultyLabel { get; set; }
		public int _editorOffset { get; set; }
		public int _editorOldOffset { get; set; }
		public string[] _warnings { get; set; }
		public string[] _information { get; set; }
		public string[] _suggestions { get; set; }
		public string[] _requirements { get; set; }
	}

	public class DifficultyBeatmap
	{
		public string _difficulty { get; set; }
		public int _difficultyRank { get; set; }
		public string _beatmapFilename { get; set; }
		public int _noteJumpMovementSpeed { get; set; }
		public int _noteJumpStartBeatOffset { get; set; }
		public CustomDifficultyData[] _customData { get; set; }
	}

	public class DifficultyBeatmapSet
	{
		public string _beatmapCharacteristicName { get; set; }
		public DifficultyBeatmap[] _difficultyBeatmaps { get; set; }
	}
}