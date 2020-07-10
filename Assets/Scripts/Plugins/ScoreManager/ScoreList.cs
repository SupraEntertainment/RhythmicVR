using System.Collections.Generic;
using System.IO;
using ScoreManager;
using UnityEngine;

namespace RhythmicVR {
	public class ScoreList {
		
		private List<Playthrough> scores = new List<Playthrough>();
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
				File.WriteAllText(path + "scores.json", JsonUtility.ToJson(new ScoreListInternal(scores.ToArray()), true));
			}
		}

		public void LoadScores() {
			if (Util.EnsureFileIntegrity(path + "scores.json")) {
				scores.AddRange(JsonUtility.FromJson<ScoreListInternal>(File.ReadAllText(path + "scores.json")).scores);
				foreach (var score in scores) {
					score.FillListsFromArrays();
				}
			}
		}

		public Playthrough AddPlaythrough() {
			var score = new Playthrough();
			scores.Add(score);
			return score;
		}

		public Playthrough[] GetScores() {
			return scores.ToArray();
		}
	}

	[System.Serializable]
	internal class ScoreListInternal {
		public Playthrough[] scores;

		public ScoreListInternal(Playthrough[] scores) {
			this.scores = scores;
		}
	}
}