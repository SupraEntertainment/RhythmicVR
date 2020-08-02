using System;
using System.IO;
using RhythmicVR;
using UnityEngine;

namespace BlockSong {
	public class BlockSong : SongType {
		
		//list all classes, to include for usage from other plugins
		[NonSerialized] public Song song;
		[NonSerialized] public Difficulty difficulty;
		[NonSerialized] public Beatmap beatmap;
		[NonSerialized] public Note note;
		[NonSerialized] public Obstacle obstacle;
		[NonSerialized] public Event event1;

		public new Beatmap LoadBeatmap(string path) {
			Beatmap bm = JsonUtility.FromJson<Beatmap>(File.ReadAllText(path));
			return bm;
		}
	}
}