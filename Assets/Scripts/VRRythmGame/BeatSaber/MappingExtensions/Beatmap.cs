using System.Collections.Generic;

namespace VRRythmGame.BeatSaber.MappingExtensions {
	[System.Serializable]
	public class Beatmap {
		private int _time = 0;
		private int[] _BPMChanges = {0};
		private BeatSaber.Beatmap.Event[] _events = new BeatSaber.Beatmap.Event[]{};
		private Note[] _notes = new Note[]{};
		private Obstacle[] _obstacles = new Obstacle[]{};
		private string[] _bookmarks = new string[]{};

		public VRRythmGame.Beatmap ToBeatmap() {
			List<Event> events = new List<Event>();
			foreach (var bsEvent in _events) {
				events.Add(bsEvent.ToEvent());
			}
			List<VRRythmGame.Note> notes = new List<VRRythmGame.Note>();
			foreach (var bsNote in _notes) {
				notes.Add(bsNote.ToNote());
			}
			List<VRRythmGame.Obstacle> obstacles = new List<VRRythmGame.Obstacle>();
			foreach (var bsObstacle in _obstacles) {
				obstacles.Add(bsObstacle.ToObstacle());
			}
			return new VRRythmGame.Beatmap(events.ToArray(), notes.ToArray(), obstacles.ToArray());
		}
	
		[System.Serializable]
		public class Note {
			private float _time;
			private int _lineIndex;
			private float _lineLayer;
			private int _type;
			private float _cutDirection;

			public VRRythmGame.Note ToNote() {
				return new VRRythmGame.Note(_time, 
				                            _lineIndex/2 -2, 
				                            _lineLayer/2f, 
				                            _type == 0 ? new []{TrackingPoint.LeftHand} : new TrackingPoint[]{TrackingPoint.RightHand}, 
				                            _cutDirection);
			}
		}
	
		[System.Serializable]
		public class Obstacle {
			private float _time;
			private int _lineIndex;
			private int _type;
			private float _duration;
			private int _width;

			public VRRythmGame.Obstacle ToObstacle() {
				return new VRRythmGame.Obstacle(_time, 
				                                (_lineIndex != 5 ? _lineIndex/2f - 2 : 0), 
				                                (_type == 0 ? 1.5f : 2f), 
				                                (_type == 0 ? 1 : 3), 
				                                _width, 
				                                _duration, 
				                                _type );
			}
		}
	}
}
