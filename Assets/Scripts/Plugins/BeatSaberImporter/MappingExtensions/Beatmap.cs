using System.Collections.Generic;
using BlockSong;
using RhythmicVR;

namespace BeatSaberImporter.MappingExtensions {
	[System.Serializable]
	public class Beatmap {
		public int _time = 0;
		public int[] _BPMChanges = {0};
		public BeatSaberImporter.Beatmap.Event[] _events = new BeatSaberImporter.Beatmap.Event[]{};
		public Note[] _notes = new Note[]{};
		public Obstacle[] _obstacles = new Obstacle[]{};
		public string[] _bookmarks = new string[]{};

		public BlockSong.Beatmap ToBeatmap() {
			List<Event> events = new List<Event>();
			foreach (var bsEvent in _events) {
				events.Add(bsEvent.ToEvent());
			}
			List<BlockSong.Note> notes = new List<BlockSong.Note>();
			foreach (var bsNote in _notes) {
				notes.Add(bsNote.ToNote());
			}
			List<BlockSong.Obstacle> obstacles = new List<BlockSong.Obstacle>();
			foreach (var bsObstacle in _obstacles) {
				obstacles.Add(bsObstacle.ToObstacle());
			}
			return new BlockSong.Beatmap(events.ToArray(), notes.ToArray(), obstacles.ToArray());
		}
	
		[System.Serializable]
		public class Note {
			public float _time;
			public int _lineIndex;
			public float _lineLayer;
			public int _type;
			public float _cutDirection;

			public BlockSong.Note ToNote() {
				var note = new BlockSong.Note();
				note.time = _time;
				note.xPos = _lineIndex / 1000f;
				note.yPos = _lineLayer / 1000f;
				note.target = new[] {_type == 0 ? TrackingPoint.LeftHand : TrackingPoint.RightHand};
				note.cutDirection = _cutDirection * 45;
				return note;
			}
		}
	
		[System.Serializable]
		public class Obstacle {
			public float _time;
			public int _lineIndex;
			public int _type;
			public float _duration;
			public int _width;

			public BlockSong.Obstacle ToObstacle() {
				return new BlockSong.Obstacle(_time, 
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
