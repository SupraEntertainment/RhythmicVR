using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using RhythmicVR;
using RhythmicVR.BeatSaber;
using SFB;

namespace BeatSaber {
    public class BeatSaberImportPlugin : PluginBaseClass {
        public override void Init(Core core) {
            base.Init(core);
            SetupUiElements();
        }

        private void LoadBeatSaberMap() {
            new Thread(OpenBeatSaberSong).Start();
        }

        private void OpenBeatSaberSong() {
            StandaloneFileBrowser.OpenFolderPanelAsync("Open beatsaber Beatmap", "", true, delegate(string[] strings) {
                foreach (var path in strings) {
                    SongLoader.ConvertSong(path, core);
                }
            });
        }

        private void LoadBeatSaberMapFolder() {
            new Thread(OpenBeatSaberSongFolder).Start();
        }

        private void OpenBeatSaberSongFolder() {
            StandaloneFileBrowser.OpenFolderPanelAsync("Open beatsaber Beatmap", "", false, delegate(string[] strings) {
                string[] paths = Directory.GetDirectories(strings[0]);
                core.uiManager.ProgressBarSetActive(true);
                core.uiManager.ProgressBarSetTitle("Importing songs");
                int i = 0;
                int max = paths.Length;
                foreach (var path in paths) {
                    try {
                        SongLoader.ConvertSong(path, core);
                    }
                    catch (Exception e) {
                        Console.WriteLine(e);
                    }

                    i++;
                    core.uiManager.ProgressBarSetValue(1/max*i);
                    core.uiManager.ProgressBarSetTitle("Importing songs " + i + "/" + max);
                }
                core.uiManager.ProgressBarSetActive(false);
            });
        }

        private void SetupUiElements() {
            List<SettingsField> settingsFields = new List<SettingsField>();

            {
                SettingsField setting = new SettingsField("Import Beatsaber Song", UiType.Button, core.uiManager.buttonPrefab, "Settings/Plugins/Beat Saber Import");
                setting.buttonCall = LoadBeatSaberMap;
                settingsFields.Add(setting);
            }

            {
                SettingsField setting = new SettingsField("Import Beatsaber Song Folder", UiType.Button, core.uiManager.buttonPrefab, "Settings/Plugins/Beat Saber Import");
                setting.buttonCall = LoadBeatSaberMapFolder;
                settingsFields.Add(setting);
            }
        
            core.settingsManager.settings.AddRange(settingsFields);
            core.settingsManager.UpdateSettingsUi();
        }
    }
}
