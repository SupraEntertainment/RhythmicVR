using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RhythmicVR {
	[System.Serializable]
	public class Score {
		public string scoringsystem;
		public float score;
		public float[] notes;
		protected int scoreMultiplier = 1;
		protected int amntPastNotesHit;
		protected List<float> noteScores = new List<float>();

		public virtual float GetScore() {
			return score;
		}

		/// <summary>
		/// Fill arrays from lists, etc...
		/// gets called just before saving
		/// </summary>
		public virtual void FinalizeScore() {
			notes = noteScores.ToArray();
		}

		public virtual void FillListsFromArrays() {
			noteScores.AddRange(notes);
		}

		/// <summary>
		/// Return the current multiplier and progress to the next one
		/// </summary>
		/// <returns>x = current multiplier, y = percentage to next multiplier (-1 = max is reached)</returns>
		public virtual Vector2 GetMultiplier() {
			return new Vector2(scoreMultiplier, PercentToNextMultiplier());
		}

		/// <summary>
		/// Calculate the percentage, to when the next multiplier is reached
		/// </summary>
		/// <returns>the % as value from 0 to 1. if maximum multiplier is reached returns -1</returns>
		protected virtual float PercentToNextMultiplier() {
			return  scoreMultiplier == 1 ? amntPastNotesHit / 4 : 
					scoreMultiplier == 2 ? amntPastNotesHit / 8 : 
					scoreMultiplier == 4 ? amntPastNotesHit / 16 : -1;
		}

		/// <summary>
		/// Add a value to the score.
		/// Override to change score calculation logic
		/// </summary>
		/// <param name="unscaledScore">The score value without any multipiers applied</param>
		public virtual void AddScore(float unscaledScore) {
			float calculatedScore;
			calculatedScore = unscaledScore * scoreMultiplier;
			noteScores.Add(calculatedScore);
			score = noteScores.Sum();

			if (unscaledScore <= 0) {
				amntPastNotesHit = 0;
				scoreMultiplier = scoreMultiplier == 8 ? 4 : scoreMultiplier == 4 ? 2 : 1;
			} else {
				amntPastNotesHit++;
			}
			
			if (scoreMultiplier == 1 && amntPastNotesHit >= 4) {
				scoreMultiplier = 2;
				amntPastNotesHit = 0;
			} else if (scoreMultiplier == 2 && amntPastNotesHit >= 8) {
				scoreMultiplier = 4;
				amntPastNotesHit = 0;
			} else if (scoreMultiplier == 4 && amntPastNotesHit >= 16) {
				scoreMultiplier = 8;
				amntPastNotesHit = 0;
			} 
		}
	}
}