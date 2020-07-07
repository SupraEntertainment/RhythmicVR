using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace RhythmicVR {
	public class ScoreList {
		
		private List<Score> scores = new List<Score>();
		public Song song;
		private string path;

		public ScoreList(Song song) {
			this.song = song;
			path = song.pathToDir;
		}

		public void SaveScores() {
			if (Util.EnsureDirectoryIntegrity(path)) {
				foreach (var score in scores) {
					score.FinalizeScore();
				}
				File.WriteAllText(path + "scores.json", JsonUtility.ToJson(new ScoreListInternal(scores.ToArray())));
			}
		}

		public void LoadScores() {
			if (Util.EnsureDirectoryIntegrity(path)) {
				scores.AddRange(JsonUtility.FromJson<ScoreListInternal>(File.ReadAllText(path + "scores.json")).scores);
				foreach (var score in scores) {
					score.FillListsFromArrays();
				}
			}
		}

		public Score AddScore() {
			var score = new Score();
			scores.Add(score);
			return score;
		}

		public Score[] GetScores() {
			return scores.ToArray();
		}
	}

	[System.Serializable]
	internal class ScoreListInternal {
		public Score[] scores;

		public ScoreListInternal(Score[] scores) {
			this.scores = scores;
		}
	}
}