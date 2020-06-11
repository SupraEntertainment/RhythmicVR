using System;
using System.Collections.Generic;
using UnityEngine;

namespace VRRythmGame.BeatSaber {
	[System.Serializable]
	public class Beatmap {
		public float _time = 0;
		public object[] _BPMChanges = {0};
		public Event[] _events = new Event[]{};
		public Note[] _notes = new Note[]{};
		public Obstacle[] _obstacles = new Obstacle[]{};
		public object[] _bookmarks = new object[]{};

		public VRRythmGame.Beatmap ToBeatmap() {
			List<VRRythmGame.Event> events = new List<VRRythmGame.Event>();
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
			public float _time;
			public int _type;
			public int _value;

			public VRRythmGame.Event ToEvent() {
				var mapEvent = new VRRythmGame.Event();
				return mapEvent;
			}
		}
		
		[System.Serializable]
		public class Note {
			public float _time;
			public int _lineIndex;
			public int _lineLayer;
			public int _type;
			public int _cutDirection;

			public VRRythmGame.Note ToNote() {
				var note = new VRRythmGame.Note();
				note.time = _time;
				note.xPos = _lineIndex -2 / 2f;
				note.yPos = _lineLayer / 2f;
				note.type = new[] {_type == 0 ? TrackingPoint.LeftHand : TrackingPoint.RightHand};
				note.cutDirection = _cutDirection;
				return note;
			}
		}
		
		[System.Serializable]
		public class Obstacle {
			private float _time;
			private int _lineIndex;
			private int _type;
			private double _duration;
			private int _width;

			public VRRythmGame.Obstacle ToObstacle() {
				return new VRRythmGame.Obstacle(_time, 
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
