using System.IO;
using UnityEngine;

namespace RhythmicVR {
	public class SongType : PluginBaseClass {

		public Beatmap LoadBeatmap(string path) {
			return JsonUtility.FromJson<Beatmap>(File.ReadAllText(path));
		}
		
	}
}