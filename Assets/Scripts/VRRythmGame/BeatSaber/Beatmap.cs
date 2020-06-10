using System;
using System.Collections.Generic;
using UnityEngine;

namespace VRRythmGame.BeatSaber {
	[System.Serializable]
	public class Beatmap {
		private double _time = 0;
		private object[] _BPMChanges = {0};
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

			Debug.Log(String.Format("{0}, {1}, {2}", events, notes, obstacles));
			return new VRRythmGame.Beatmap(events.ToArray(), notes.ToArray(), obstacles.ToArray());
		}
		
		[System.Serializable]
		public class Event {
			private double _time;
			private int _type;
			private double _value;

			public MapEvent ToEvent() {
				var mapEvent = new MapEvent();
				return mapEvent;
			}
		}
		
		[System.Serializable]
		public class Note {
			private double _time;
			private int _lineIndex;
			private int _lineLayer;
			private int _type;
			private int _cutDirection;

			public VRRythmGame.Note ToNote() {
				return new VRRythmGame.Note(float.Parse(_time.ToString()), 
				                            _lineIndex/2 -2, 
				                            _lineLayer/2f, 
				                            new []{_type == 0 ? TrackingPoint.LeftHand : TrackingPoint.RightHand}, 
				                            _cutDirection);
			}
		}
		
		[System.Serializable]
		public class Obstacle {
			private double _time;
			private int _lineIndex;
			private int _type;
			private double _duration;
			private int _width;

			public VRRythmGame.Obstacle ToObstacle() {
				return new VRRythmGame.Obstacle(float.Parse(_time.ToString()), 
				                                (_lineIndex != 5 ? _lineIndex/2f - 2 : 0), 
				                                (_type == 0 ? 1.5f : 2f), 
				                                (_type == 0 ? 1 : 3), 
				                                _width, 
				                                float.Parse(_duration.ToString()), 
				                                _type );
			}
		}
	}
}
