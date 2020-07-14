using System.Collections.Generic;
using RhythmicVR;
using UnityEngine;
using UnityEngine.UI;

namespace ScoreManager {
	public class ScoreManager : PluginBaseClass {

		public GameObject scoreListPrefab;
		public GameObject scorePrefab;
		public Config scoreConfig;
		public Playthrough currentPlaythrough;
		
		private ScoreList scoreList;
		private RectTransform scoreListContent;
		
		public override void Init(Core core) {
			base.Init(core);
			if (core.config.LoadPluginConfig(pluginName) != null) {
				scoreConfig = JsonUtility.FromJson<Config>(core.config.LoadPluginConfig(pluginName));
			} else {
				scoreConfig = new Config();
				SaveConfig();
			}
			
			SetupScoreList();

			SetupUiElements();
		}

		private void SetupScoreList() {
			var instantiated = Instantiate(scoreListPrefab, new Vector3(3.8f, 1.5f, 2.17f), Quaternion.Euler(0, 45, 0), core.uiManager.songList.transform);
			instantiated.GetComponentInChildren<Text>().text = "Scores";
			scoreListContent = instantiated.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<RectTransform>();
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

        private void SetRelativeSaving(bool state) {
            scoreConfig.shouldSaveInSongDir = state;
            SaveConfig();
        }

        private void SetScoreSavePath(string path) {
            scoreConfig.globalScoreSavePath = path;
            SaveConfig();
        }

        private void SetShowOverlay(bool state) {
	        scoreConfig.showOverlay = state;
	        SaveConfig();
        }

        private void SetShowMultiplier(bool state) {
	        scoreConfig.showMultiplier = state;
	        SaveConfig();
        }

        private void SetShowOverallScore(bool state) {
	        scoreConfig.showOverallScore = state;
	        SaveConfig();
        }

        private void SetShowCurrentTargetScore(bool state) {
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

		public void DisplayScoresOnScoreboard(Song song) {
			if ((object) scoreList == null || scoreList.song != song) {
				scoreList = new ScoreList(song);
				scoreList.LoadScores();
			}

			for (int i = 0; i < scoreListContent.childCount; i++) {
				Destroy(scoreListContent.GetChild(i).gameObject);
			}
			
			int contentHeight = 0;
			foreach (var playthrough in scoreList.GetScores()) {
				var scorePanel = Instantiate(scorePrefab, scoreListContent);
				var texts = scorePanel.GetComponentsInChildren<Text>();
				var rt = scorePanel.GetComponent<RectTransform>(); //get rect transform
				rt.anchoredPosition = new Vector2(rt.anchoredPosition.x, -contentHeight); // set elements position
				texts[0].text = core.playerName;
				texts[1].text = playthrough.score.ToString();
				contentHeight += (int)rt.sizeDelta.y;
			}
			scoreListContent.sizeDelta = new Vector2(0, contentHeight);
		}

		public Playthrough[] GetScores(Song song) {
			if (scoreList.song != song) {
				scoreList = new ScoreList(song);
			}

			return scoreList.GetScores();
		}

		public void SaveCurrentScores() {
			if ((object)scoreList != null) {
				scoreList.SaveScores();
			}
		}
	}
}