using System;
using System.IO;
using UnityEngine;

namespace RhythmicVR {

	/// <summary>
	/// config object containing all stored values
	/// </summary>
	[Serializable]
	public class Config {
        
		public string appData;
		public string songSavePath;
		public string latestSongSortSetting;
		public int generalVolume = 50;
		public int menuVolume = 50;
		public int songVolume = 50;
		public int songPreviewVolume = 50;
		public int hitVolume = 50;
		public int missVolume = 50;
		public int wrongHitVolume = 50;

		public Config() {
			if (appData == null) {
				appData = Application.consoleLogPath.Substring(0, Application.consoleLogPath.Length - 10);
			}

			if (songSavePath == null) {
				songSavePath = appData + "songs/";
			}
		}

		/// <summary>
		/// Save config file
		/// </summary>
		public void Save() {
			File.WriteAllText(appData + "/config.json", JsonUtility.ToJson(this, true));
		}

		/// <summary>
		/// load config file from path
		/// </summary>
		/// <param name="path">Path to load the file from</param>
		/// <returns>The config object from that file</returns>
		public static Config Load(string path) {
			return JsonUtility.FromJson<Config>(File.ReadAllText(path + "/config.json"));
		}
	}
}