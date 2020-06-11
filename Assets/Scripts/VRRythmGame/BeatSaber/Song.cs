using System.Collections.Generic;

namespace VRRythmGame.BeatSaber {
	
	[System.Serializable]
	public class Song {
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
		public CustomData _customData = new CustomData();
		public DifficultyBeatmapSet[] _difficultyBeatmapSets = new DifficultyBeatmapSet[]{};

		public VRRythmGame.Song ToSong() {
			var song = new VRRythmGame.Song();
			song.songName = _songName;
			song.environment = _environmentName;
			song.songAuthorName = _songAuthorName;
			song.levelAuthorName = _levelAuthorName;
			song.trackingPoints = new TrackingPoint[] {TrackingPoint.LeftHand, TrackingPoint.RightHand};
			song.albumName = _songSubName;
			song.beatsPerMinute = _beatsPerMinute;
			song.previewStartTime = float.Parse(_previewStartTime.ToString());
			song.songFile = _songFilename;
			song.coverImageFile = _coverImageFilename;
			song.startTimeOffset = (float)_songTimeOffset;
			List<Difficulty> difficulties = new List<Difficulty>();
			
			foreach (var difficulty in _difficultyBeatmapSets) {
				foreach (var difficultyBeatmap in difficulty._difficultyBeatmaps) {
					Difficulty diffic = new Difficulty();
					diffic.name = difficultyBeatmap._difficulty;
					diffic.difficulty = difficultyBeatmap._difficultyRank;
					diffic.beatMapAuthor = _levelAuthorName;
					diffic.beatMapPath = difficultyBeatmap._beatmapFilename;
					difficulties.Add(diffic);
				}
			}
			song.difficulties = difficulties.ToArray();
			
			return song;
		}
	}
	
	[System.Serializable]
	public class CustomData
	{
		public string _customEnvironment;
		public string _customEnvironmentHash;
		public object[] _contributors = new object[]{};
	}

	[System.Serializable]
	public class CustomDifficultyData {
		public string _difficultyLabel;
		public int _editorOffset;
		public int _editorOldOffset;
		public string[] _warnings = new string[]{};
		public string[] _information = new string[]{};
		public string[] _suggestions = new string[]{};
		public string[] _requirements = new string[]{};
	}

	[System.Serializable]
	public class DifficultyBeatmap
	{
		public string _difficulty;
		public int _difficultyRank;
		public string _beatmapFilename;
		public int _noteJumpMovementSpeed;
		public int _noteJumpStartBeatOffset;
		public CustomDifficultyData _customData;
	}

	[System.Serializable]
	public class DifficultyBeatmapSet
	{
		public string _beatmapCharacteristicName;
		public DifficultyBeatmap[] _difficultyBeatmaps = new DifficultyBeatmap[]{};
	}
}