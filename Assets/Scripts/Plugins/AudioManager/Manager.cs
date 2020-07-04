using System.Collections.Generic;
using RhythmicVR;
using UnityEngine;

namespace AudioManager {
    [System.Serializable]
    public class Manager : PluginBaseClass {

        private Config audioConfig;
    
        public override void Init(Core core) {
            base.Init(core);
            if (core.config.LoadPluginConfig(pluginName) != null) {
                audioConfig = JsonUtility.FromJson<Config>(core.config.LoadPluginConfig(pluginName));
            } else {
                audioConfig = new Config();
                SaveConfig();
            }

            SetupUiElements();
        }

        private void SetupUiElements() {
            List<SettingsField> audioSettings = new List<SettingsField>();

            {
                SettingsField setting = new SettingsField("General", UiType.Int, core.uiManager.intPrefab, "Settings/Audio");
                setting.call.AddListener(delegate { SetGeneralVolume(int.Parse(setting._input)); });
                audioSettings.Add(setting);
            }

            {
                SettingsField setting = new SettingsField("Menu", UiType.Int, core.uiManager.intPrefab, "Settings/Audio");
                setting.call.AddListener(delegate { SetMenuVolume(int.Parse(setting._input)); });
                audioSettings.Add(setting);
            }
            
            {
                SettingsField setting = new SettingsField("Song", UiType.Int, core.uiManager.intPrefab, "Settings/Audio");
                setting.call.AddListener(delegate { SetSongVolume(int.Parse(setting._input)); });
                audioSettings.Add(setting);
            }
            
            {
                SettingsField setting = new SettingsField("Song Preview", UiType.Int, core.uiManager.intPrefab, "Settings/Audio");
                setting.call.AddListener(delegate { SetSongPreviewVolume(int.Parse(setting._input)); });
                audioSettings.Add(setting);
            }
            
            {
                SettingsField setting = new SettingsField("Hit", UiType.Int, core.uiManager.intPrefab, "Settings/Audio");
                setting.call.AddListener(delegate { SetHitVolume(int.Parse(setting._input)); });
                audioSettings.Add(setting);
            }
            
            {
                SettingsField setting = new SettingsField("Miss", UiType.Int, core.uiManager.intPrefab, "Settings/Audio");
                setting.call.AddListener(delegate { SetMissVolume(int.Parse(setting._input)); });
                audioSettings.Add(setting);
            }
            
            {
                SettingsField setting = new SettingsField("Wrong Hit", UiType.Int, core.uiManager.intPrefab, "Settings/Audio");
                setting.call.AddListener(delegate { SetWrongHitVolume(int.Parse(setting._input)); });
                audioSettings.Add(setting);
            }
            
            core.settingsManager.settings.AddRange(audioSettings);
            core.settingsManager.UpdateSettingsUi();
        }

        private void SaveConfig() {
            core.config.SavePluginConfig(JsonUtility.ToJson(audioConfig), pluginName);
        }

        public void SetGeneralVolume(int volume) {
            audioConfig.generalVolume = volume;
            SaveConfig();
        }

        public void SetMenuVolume(int volume) {
            audioConfig.menuVolume = volume;
            SaveConfig();
        }

        public void SetSongVolume(int volume) {
            audioConfig.songVolume = volume;
            SaveConfig();
        }

        public void SetSongPreviewVolume(int volume) {
            audioConfig.songPreviewVolume = volume;
            SaveConfig();
        }

        public void SetHitVolume(int volume) {
            audioConfig.hitVolume = volume;
            SaveConfig();
        }

        public void SetMissVolume(int volume) {
            audioConfig.missVolume = volume;
            SaveConfig();
        }

        public void SetWrongHitVolume(int volume) {
            audioConfig.wrongHitVolume = volume;
            SaveConfig();
        }
    }
}
