using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace RhythmicVR {
	public class ScoreList {
		
		private List<Score> scores = new List<Score>();
		private string path;

		public ScoreList(Song song) {
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

		public List<Score> GetScores() {
			return scores;
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