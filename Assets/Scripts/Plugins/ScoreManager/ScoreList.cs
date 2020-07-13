using System.Collections.Generic;
using System.IO;
using ScoreManager;
using UnityEngine;

namespace RhythmicVR {
	public class ScoreList {
		
		private List<Playthrough> playthroughs = new List<Playthrough>();
		public Song song;
		private string path;

		public ScoreList(Song song) {
			this.song = song;
			path = song.pathToDir;
		}

		public void SaveScores() {
			if (Util.EnsureDirectoryIntegrity(path)) {
				foreach (var score in playthroughs) {
					score.FinalizeScore();
				}
				File.WriteAllText(path + "scores.json", JsonUtility.ToJson(new ScoreListInternal(playthroughs.ToArray()), true));
			}
		}

		public void LoadScores() {
			if (Util.EnsureFileIntegrity(path + "scores.json")) {
				playthroughs.AddRange(JsonUtility.FromJson<ScoreListInternal>(File.ReadAllText(path + "scores.json")).playthroughs);
				foreach (var score in playthroughs) {
					score.FillListsFromArrays();
				}
			}
		}

		public Playthrough AddPlaythrough() {
			var playthrough = new Playthrough();
			playthroughs.Add(playthrough);
			return playthrough;
		}

		public Playthrough[] GetScores() {
			return playthroughs.ToArray();
		}
	}

	[System.Serializable]
	internal class ScoreListInternal {
		public Playthrough[] playthroughs;

		public ScoreListInternal(Playthrough[] playthroughs) {
			this.playthroughs = playthroughs;
		}
	}
}