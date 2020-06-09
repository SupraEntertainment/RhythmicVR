using SFB;
using UnityEngine;

namespace VRRythmGame {
    public class Buttonfunctions : MonoBehaviour {

        public GameManager gm;
    
        public void LoadBeatSaberMap() {
        
            StandaloneFileBrowser.OpenFilePanelAsync("Open beatsaber Beatmap", "", "", true, delegate(string[] strings) {
                foreach (var path in strings) {
                    gm.LoadSong(BeatSaber.SongLoader.ConvertBeatmap(path));
                }
            });
        }
    }
}
