using System.Collections.Generic;

namespace BeatSaber {
	[System.Serializable]
	public class BSNEBeatmap {
		private int _time;
		private int[] _BPMChanges;
		private BSEvent[] _events;
		private BSNote[] _notes;
		private BSObstacle[] _obstacles;
		private string[] _bookmarks;

		public Beatmap ToBeatmap() {
			List<Event> events = new List<Event>();
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
