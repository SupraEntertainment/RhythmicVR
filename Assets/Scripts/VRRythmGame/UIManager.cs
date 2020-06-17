using System;
using System.Collections.Generic;
using System.IO;
using SFB;
using UnityEngine;
using UnityEngine.UI;

namespace VRRythmGame {
    public class UIManager : MonoBehaviour {

        public GameManager gm;
        public RectTransform songListParent;
        
        [Header("UI Prefabs")]
        public GameObject songListItem;

        public void Start() {
            List<Song> songs = new List<Song>();
            string[] paths = {"", ""}; 
            foreach (var path in paths) {
                songs.Add(GameManager.ReadSongFromFile(path));
            }
            
            ListAllSongs(songListParent, songs, songListItem);
        }

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

        public void ListAllSongs(RectTransform parent, List<Song> songs, GameObject buttonPrefab) {
            foreach (var song in songs) {
                GameObject button = Instantiate(buttonPrefab, parent);
                button.GetComponentInChildren<Text>().text = song.songName + " - " + song.songAuthorName + "\n" + song.songSubName;
            }
        }
    }
}
