using System.Collections.Generic;

namespace VRRythmGame.BeatSaber {
	[System.Serializable]
	public class BSBeatmap {
		private int _time = 0;
		private int[] _BPMChanges = {0};
		private BSEvent[] _events = new BSEvent[]{};
		private BSNote[] _notes = new BSNote[]{};
		private BSObstacle[] _obstacles = new BSObstacle[]{};
		private string[] _bookmarks = new string[]{};

		public Beatmap ToBeatmap() {
			List<MapEvent> events = new List<MapEvent>();
			foreach (var bsEvent in _events) {
				events.Add(bsEvent.ToEvent());
			}
			List<Note> notes = new List<Note>();
			foreach (var bsNote in _notes) {
				notes.Add(bsNote.ToNote());
			}
			List<Obstacle> obstacles = new List<Obstacle>();
			foreach (var bsObstacle in _obstacles) {
				obstacles.Add(bsObstacle.ToObstacle());
			}
			return new Beatmap(events.ToArray(), notes.ToArray(), obstacles.ToArray());
		}
	}
}
