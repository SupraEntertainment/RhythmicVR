using System.IO;
using SFB;
using UnityEngine;

namespace VRRythmGame {
    public class Buttonfunctions : MonoBehaviour {

        public GameManager gm;
    
        public void LoadBeatSaberMap() {
            StandaloneFileBrowser.OpenFolderPanelAsync("Open beatsaber Beatmap", "", true, delegate(string[] strings) {
                foreach (var path in strings) {
                    gm.LoadSong(BeatSaber.SongLoader.ConvertSong(path, gm));
                }
            });
        }
    
        public void LoadBeatmap() {
            StandaloneFileBrowser.OpenFolderPanelAsync("Load and Play Beatmap", "", true, delegate(string[] strings) {
                foreach (var path in strings) {
                    gm.LoadSong(path + Path.DirectorySeparatorChar);
                }
            });
        }
    }
}
