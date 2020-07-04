using System.Collections;
using System.Collections.Generic;
using System.Threading;
using RhythmicVR;
using RhythmicVR.BeatSaber;
using SFB;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class BeatSaberImportPlugin : PluginBaseClass {
	public override void Init(Core core) {
		base.Init(core);
        SetupUiElements();
	}

    public void LoadBeatSaberMap() {
        new Thread(OpenBeatSaberSong).Start();
    }

    private void OpenBeatSaberSong() {
        StandaloneFileBrowser.OpenFolderPanelAsync("Open beatsaber Beatmap", "", true, delegate(string[] strings) {
            foreach (var path in strings) {
                core.LoadSong(SongLoader.ConvertSong(path, core));
            }
        });
    }

    private void SetupUiElements() {
        List<SettingsField> audioSettings = new List<SettingsField>();

        {
            SettingsField setting = new SettingsField("Convert and Import Beatsaber Map", UiType.Button, core.uiManager.intPrefab, "Settings/Plugins/Beat Saber Import");
            setting.call.AddListener(LoadBeatSaberMap);
            audioSettings.Add(setting);
        }
        
        core.settingsManager.settings.AddRange(audioSettings);
        core.settingsManager.UpdateSettingsUi();
    }
}
