using System;
using System.Collections.Generic;
using System.IO;
using SFB;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;

namespace VRRythmGame {
    public class UIManager : MonoBehaviour {

        public GameManager gm;
        public RectTransform songListParent;
        public SteamVR_ActionSet uiActionSet;

        [Header("Menus")] 
        public GameObject songList;
        public GameObject mainMenu;
        public GameObject settingsMenu;
        
        [Header("UI Prefabs")]
        public GameObject songListItem;

        private List<GameObject> _loadedSongs = new List<GameObject>();

        public void Start() {
            uiActionSet.Activate();
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
        
        // menu navigation

        public void ToMainMenu() {
            songList.SetActive(false);
            mainMenu.SetActive(true);
            settingsMenu.SetActive(false);
        }

        public void ToSettingsMenu() {
            songList.SetActive(false);
            mainMenu.SetActive(false);
            settingsMenu.SetActive(true);
        }

        public void ToSongListMenu() {
            songList.SetActive(true);
            mainMenu.SetActive(false);
            settingsMenu.SetActive(false);
            ListSongs(gm.songList.GetAllSongs());
        }

        public void Quit() {
            Application.Quit();
        }
        
        /*-------------------- ^^ button methods ^^ --------------------------*/
        /*-------------------- vv songlist vv --------------------------*/

        public void ListSongs(List<Song> songs) {
            ListAllSongs(songListParent, songs, songListItem);
        }

        private void ListAllSongs(RectTransform parent, List<Song> songs, GameObject buttonPrefab) {
            if (_loadedSongs.Count != 0) {
                RemoveAllListedSongs();
            }

            var prt = parent.GetComponent<RectTransform>();
            prt.sizeDelta = new Vector2(0, 20 + songs.Count * (buttonPrefab.GetComponent<RectTransform>().rect.height+20));
            for (var i = 0; i < songs.Count; i++) {
                var song = songs[i];
                GameObject button = Instantiate(buttonPrefab, parent);
                button.GetComponentInChildren<Text>().text = song.songName + " - " + song.songAuthorName + "\n" + song.songSubName;
                var rt = button.GetComponent<RectTransform>();
                button.GetComponent<RectTransform>().anchoredPosition = new Vector2(rt.anchoredPosition.x, -20 - i * (rt.rect.height+20));
                _loadedSongs.Add(button);
            }
        }

        private void RemoveAllListedSongs() {
            foreach (var loadedSong in _loadedSongs) {
                Destroy(loadedSong);
            }
        }
    }
}
