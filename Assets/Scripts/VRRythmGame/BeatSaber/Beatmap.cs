using System.Collections.Generic;

namespace VRRythmGame.BeatSaber {
	[System.Serializable]
	public class Beatmap {
		private int _time = 0;
		private int[] _BPMChanges = {0};
		private Event[] _events = new Event[]{};
		private Note[] _notes = new Note[]{};
		private Obstacle[] _obstacles = new Obstacle[]{};
		private object[] _bookmarks = new object[]{};

		public VRRythmGame.Beatmap ToBeatmap() {
			List<MapEvent> events = new List<MapEvent>();
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
		public class Event {
			private float _time;
			private int _type;
			private int _value;

			public MapEvent ToEvent() {
				var mapEvent = new MapEvent();
				return mapEvent;
			}
		}
		
		[System.Serializable]
		public class Note {
			private float _time;
			private int _lineIndex;
			private int _lineLayer;
			private int _type;
			private int _cutDirection;

			public VRRythmGame.Note ToNote() {
				return new VRRythmGame.Note(_time, 
				                            _lineIndex/2 -2, 
				                            _lineLayer/2f, 
				                            new []{_type == 0 ? TrackingPoint.LeftHand : TrackingPoint.RightHand}, 
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
