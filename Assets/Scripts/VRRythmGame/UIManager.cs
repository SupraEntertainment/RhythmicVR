using System.Collections.Generic;
using System.IO;
using System.Threading;
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
        public GameObject pauseMenu;

        [Header("Song Info Panel elements")]
        public Button playBeatmapButton;
        public Button deleteBeatmapButton;
        public Button practiceBeatmapButton;
        public Text beatmapTitleText;
        public Image beatmapCoverImage;
        
        [Header("UI Prefabs")]
        public GameObject songListItem;

        private List<GameObject> _loadedSongs = new List<GameObject>();

        public void Start() {
            uiActionSet.Activate();
            ToMainMenu();
        }

        public void LoadBeatSaberMap() {
            new Thread(OpenBeatSaberSong).Start();
        }
    
        public void LoadSong() {
            new Thread(OpenSongFile).Start();
        }
    
        public void OpenSongFile() {
            StandaloneFileBrowser.OpenFolderPanelAsync("Load and Play Beatmap", "", true, delegate(string[] strings) {
                foreach (var path in strings) {
                    gm.LoadSong(path + Path.DirectorySeparatorChar);
                }
            });
        }

        private void OpenBeatSaberSong() {
            StandaloneFileBrowser.OpenFolderPanelAsync("Open beatsaber Beatmap", "", true, delegate(string[] strings) {
                foreach (var path in strings) {
                    gm.LoadSong(BeatSaber.SongLoader.ConvertSong(path, gm));
                }
            });
        }
        
        // menu navigation
        
        public void ShowPauseMenu(int reason) {
            switch (reason) {
                case 0: // paused
                    pauseMenu.transform.Find("Canvas/Panel/Btn_resume").gameObject.SetActive(true);
                    break;
                case 1: // completed
                case 2: // failed
                    pauseMenu.transform.Find("Canvas/Panel/Btn_resume").gameObject.SetActive(false);
                    break;
                default:
                    break;
            }
            pauseMenu.SetActive(true);
        }
        
        public void HidePauseMenu() {
            pauseMenu.SetActive(false);
        }

        public void NoMenu() {
            songList.SetActive(false);
            mainMenu.SetActive(false);
            settingsMenu.SetActive(false);
        }

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
                button.transform.Find("Image").GetComponent<Image>().sprite = Util.LoadSprite(song.pathToDir + song.coverImageFile);
                var rt = button.GetComponent<RectTransform>();
                button.GetComponent<RectTransform>().anchoredPosition = new Vector2(rt.anchoredPosition.x, -20 - i * (rt.rect.height+20));
                button.GetComponent<Button>().onClick.AddListener(delegate { DisplaySongInfo(song); });
                _loadedSongs.Add(button);
            }
        }

        private void RemoveAllListedSongs() {
            foreach (var loadedSong in _loadedSongs) {
                Destroy(loadedSong);
            }
        }
        
        /* Display song info */

        public void DisplaySongInfo(Song song) {
            Debug.Log("Displaying Song " + song.songName + " by " + song.songAuthorName);
            
            // remove listeners from previous song
            playBeatmapButton.onClick.RemoveAllListeners();
            deleteBeatmapButton.onClick.RemoveAllListeners();
            practiceBeatmapButton.onClick.RemoveAllListeners();
            
            // add listeners
            playBeatmapButton.onClick.AddListener(delegate { gm.StartBeatmap(song, song.difficulties[0], null); });
            deleteBeatmapButton.onClick.AddListener(delegate {  });
            practiceBeatmapButton.onClick.AddListener(delegate { gm.StartBeatmap(song, song.difficulties[0], null); });
            
            // set text
            beatmapTitleText.text = song.songName + " - " + song.songAuthorName + "\n" + song.songSubName;
            beatmapCoverImage.sprite = Util.LoadSprite(song.pathToDir + song.coverImageFile);
        }
    }
}
