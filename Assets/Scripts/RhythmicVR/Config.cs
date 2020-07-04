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
		public string pluginSavePath;
		public string latestSongSortSetting;

		public Config() {
			if (appData == null) {
				appData = Application.consoleLogPath.Substring(0, Application.consoleLogPath.Length - 10);
			}

			if (songSavePath == null) {
				songSavePath = appData + "songs/";
			}

			if (pluginSavePath == null) {
				pluginSavePath = appData + "plugins/";
			}
		}

		public void SavePluginConfig(string contents, string pluginName) {
			var path = pluginSavePath + pluginName + "/";
			if (Util.EnsureDirectoryIntegrity(path, true)) {
				File.WriteAllText(path + "config.json", contents);
			}
		}

		public string LoadPluginConfig(string pluginName) {
			var path = pluginSavePath + pluginName + "/";
			if (Util.EnsureDirectoryIntegrity(path)) {
				return File.ReadAllText(path + "config.json");
			}
			return null;
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