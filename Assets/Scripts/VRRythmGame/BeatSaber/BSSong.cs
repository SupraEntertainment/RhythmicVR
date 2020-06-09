namespace VRRythmGame.BeatSaber {
	
	[System.Serializable]
	public class BSSong {
		public string _version;
		public string _songName;
		public string _songSubName;
		public string _songAuthorName;
		public string _levelAuthorName;
		public int _beatsPerMinute;
		public int _shuffle;
		public double _shufflePeriod;
		public double _previewStartTime;
		public int _previewDuration;
		public string _songFilename;
		public string _coverImageFilename;
		public string _environmentName;
		public int _songTimeOffset;
		public CustomData _customData;
		public DifficultyBeatmapSet[] _difficultyBeatmapSets;

		public Song ToSong() {
			return new Song();
		}
	}
	
	[System.Serializable]
	public class CustomData
	{
		public string _customEnvironment;
		public string _customEnvironmentHash;
		public object[] _contributors;
	}

	[System.Serializable]
	public class CustomDifficultyData {
		public string _difficultyLabel;
		public int _editorOffset;
		public int _editorOldOffset;
		public string[] _warnings;
		public string[] _information;
		public string[] _suggestions;
		public string[] _requirements;
	}

	[System.Serializable]
	public class DifficultyBeatmap
	{
		public string _difficulty;
		public int _difficultyRank;
		public string _beatmapFilename;
		public int _noteJumpMovementSpeed;
		public int _noteJumpStartBeatOffset;
		public CustomDifficultyData[] _customData;
	}

	[System.Serializable]
	public class DifficultyBeatmapSet
	{
		public string _beatmapCharacteristicName;
		public DifficultyBeatmap[] _difficultyBeatmaps;
	}
}