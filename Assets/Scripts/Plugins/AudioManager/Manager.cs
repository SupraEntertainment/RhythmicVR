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
                setting.floatCall = delegate(float arg0) { SetGeneralVolume((int)arg0); };
                setting.stringCall = delegate(string arg0) { SetGeneralVolume(int.Parse(arg0)); };
                setting.maxValue = 100;
                setting.minValue = 0;
                audioSettings.Add(setting);
            }

            {
                SettingsField setting = new SettingsField("Menu", UiType.Int, core.uiManager.intPrefab, "Settings/Audio");
                setting.floatCall = delegate(float arg0) { SetMenuVolume((int)arg0); };
                setting.stringCall = delegate(string arg0) { SetMenuVolume(int.Parse(arg0)); };
                setting.maxValue = 100;
                setting.minValue = 0;
                audioSettings.Add(setting);
            }
            
            {
                SettingsField setting = new SettingsField("Song", UiType.Int, core.uiManager.intPrefab, "Settings/Audio");
                setting.floatCall = delegate(float arg0) { SetSongVolume((int)arg0); };
                setting.stringCall = delegate(string arg0) { SetSongVolume(int.Parse(arg0)); };
                setting.maxValue = 100;
                setting.minValue = 0;
                audioSettings.Add(setting);
            }
            
            {
                SettingsField setting = new SettingsField("Song Preview", UiType.Int, core.uiManager.intPrefab, "Settings/Audio");
                setting.floatCall = delegate(float arg0) { SetSongPreviewVolume((int)arg0); };
                setting.stringCall = delegate(string arg0) { SetSongPreviewVolume(int.Parse(arg0)); };
                setting.maxValue = 100;
                setting.minValue = 0;
                audioSettings.Add(setting);
            }
            
            {
                SettingsField setting = new SettingsField("Hit", UiType.Int, core.uiManager.intPrefab, "Settings/Audio");
                setting.floatCall = delegate(float arg0) { SetHitVolume((int)arg0); };
                setting.stringCall = delegate(string arg0) { SetHitVolume(int.Parse(arg0)); };
                setting.maxValue = 100;
                setting.minValue = 0;
                audioSettings.Add(setting);
            }
            
            {
                SettingsField setting = new SettingsField("Miss", UiType.Int, core.uiManager.intPrefab, "Settings/Audio");
                setting.floatCall = delegate(float arg0) { SetMissVolume((int)arg0); };
                setting.stringCall = delegate(string arg0) { SetMissVolume(int.Parse(arg0)); };
                setting.maxValue = 100;
                setting.minValue = 0;
                audioSettings.Add(setting);
            }
            
            {
                SettingsField setting = new SettingsField("Wrong Hit", UiType.Int, core.uiManager.intPrefab, "Settings/Audio");
                setting.floatCall = delegate(float arg0) { SetWrongHitVolume((int)arg0); };
                setting.stringCall = delegate(string arg0) { SetWrongHitVolume(int.Parse(arg0)); };
                setting.maxValue = 100;
                setting.minValue = 0;
                audioSettings.Add(setting);
            }
            
            core.settingsManager.settings.AddRange(audioSettings);
            core.settingsManager.UpdateSettingsUi();
        }

        private void SaveConfig() {
            core.config.SavePluginConfig(JsonUtility.ToJson(audioConfig, true), pluginName);
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
