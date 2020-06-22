using System;
using System.IO;
using UnityEngine;

namespace VRRythmGame {

	/*
	 * config object containing all stored values
	 */
	[Serializable]
	public class Config {
        
		public string appData;
		public string songSavePath;
		public string latestSongSortSetting;

		public Config() {
			if (appData == null) {
				appData = Application.consoleLogPath.Substring(0, Application.consoleLogPath.Length - 10);
			}

			if (songSavePath == null) {
				songSavePath = appData + "songs" + Path.DirectorySeparatorChar;
			}
		}

		public void Save() {
			File.WriteAllText(appData + Path.DirectorySeparatorChar + "config.json", JsonUtility.ToJson(this, true));
		}

		public static Config Load(string path) {
			return JsonUtility.FromJson<Config>(File.ReadAllText(path + Path.DirectorySeparatorChar + "config.json"));
		}
	}
}