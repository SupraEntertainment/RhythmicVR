using System.Collections.Generic;
using RhythmicVR;
using UnityEngine;

namespace ScoreManager {
	public class ScoreManager : PluginBaseClass {

		public Config scoreConfig;
		private ScoreList scoreList;
		public Playthrough currentPlaythrough;
		
		public override void Init(Core core) {
			base.Init(core);
			if (core.config.LoadPluginConfig(pluginName) != null) {
				scoreConfig = JsonUtility.FromJson<Config>(core.config.LoadPluginConfig(pluginName));
			} else {
				scoreConfig = new Config();
				SaveConfig();
			}

			SetupUiElements();
		}

        private void SetupUiElements() {
            List<SettingsField> audioSettings = new List<SettingsField>();

            {
                SettingsField setting = new SettingsField("Save Scores in Song Folders", UiType.Bool, core.uiManager.boolPrefab, "Settings/Files");
                setting.boolCall = SetRelativeSaving;
                audioSettings.Add(setting);
            }

            {
                SettingsField setting = new SettingsField("Score Save Path", UiType.Text, core.uiManager.textPrefab, "Settings/Files");
                setting.stringCall = SetScoreSavePath;
                audioSettings.Add(setting);
            }

            {
	            SettingsField setting = new SettingsField("Show Score Overlay in Game", UiType.Bool, core.uiManager.boolPrefab, "Settings/Plugins/Score Manager");
	            setting.boolCall = SetShowOverlay;
	            audioSettings.Add(setting);
            }

            {
	            SettingsField setting = new SettingsField("Show Multiplier", UiType.Bool, core.uiManager.boolPrefab, "Settings/Plugins/Score Manager");
	            setting.boolCall = SetShowMultiplier;
	            audioSettings.Add(setting);
            }

            {
	            SettingsField setting = new SettingsField("Show Overall Score", UiType.Bool, core.uiManager.boolPrefab, "Settings/Plugins/Score Manager");
	            setting.boolCall = SetShowOverallScore;
	            audioSettings.Add(setting);
            }

            {
	            SettingsField setting = new SettingsField("Show Current Target Score", UiType.Bool, core.uiManager.boolPrefab, "Settings/Plugins/Score Manager");
	            setting.boolCall = SetShowCurrentTargetScore;
	            audioSettings.Add(setting);
            }
            
            core.settingsManager.settings.AddRange(audioSettings);
            core.settingsManager.UpdateSettingsUi();
        }

        private void SaveConfig() {
            core.config.SavePluginConfig(JsonUtility.ToJson(scoreConfig), pluginName);
        }

        public void SetRelativeSaving(bool state) {
            scoreConfig.shouldSaveInSongDir = state;
            SaveConfig();
        }

        public void SetScoreSavePath(string path) {
            scoreConfig.globalScoreSavePath = path;
            SaveConfig();
        }

        public void SetShowOverlay(bool state) {
	        scoreConfig.showOverlay = state;
	        SaveConfig();
        }

        public void SetShowMultiplier(bool state) {
	        scoreConfig.showMultiplier = state;
	        SaveConfig();
        }

        public void SetShowOverallScore(bool state) {
	        scoreConfig.showOverallScore = state;
	        SaveConfig();
        }

        public void SetShowCurrentTargetScore(bool state) {
	        scoreConfig.showCurrentTargetScore = state;
	        SaveConfig();
        }

		public void SelectSongAndAddPlaythrough(Song song) {
			if ((object) scoreList == null || scoreList.song != song) {
				scoreList = new ScoreList(song);
				scoreList.LoadScores();
			} else {
				scoreList.SaveScores();
			}

			currentPlaythrough = scoreList.AddPlaythrough();
		}

		public Playthrough[] GetScores(Song song) {
			if (scoreList.song != song) {
				scoreList = new ScoreList(song);
			}

			return scoreList.GetScores();
		}
	}
}