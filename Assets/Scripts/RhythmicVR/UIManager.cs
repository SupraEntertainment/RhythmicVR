using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using SFB;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;

namespace RhythmicVR {
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

        [Header("In Game Panels")] 
        public List<GameObject> inGamePanels;

        private Text scoreText;
        private Text progrssText;
        private Image progressBar;
        private Text multiplierText;
        
        [Header("UI Prefabs")]
        public GameObject songListItem;
        public GameObject scrollList;
        public GameObject buttonPrefab;
        public GameObject sliderPrefab;
        public GameObject intPrefab;
        public GameObject floatPrefab;
        public GameObject textPrefab;
        public GameObject colorPrefab;
        public GameObject categoryPrefab;
        public GameObject enumPrefab;

        private List<GameObject> _loadedSongs = new List<GameObject>();

        public void Start() {
            uiActionSet.Activate();
            ToMainMenu();
            FindInGamePanels();
            PopulateInGamePanels();
        }

        public void Update() {
            PopulateInGamePanels();
        }

        // song file loading, beat saber code belongs into beatsaber import plugin, load single song file should belong into its own plugin aswell

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
        
        // pause and unpause
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

        // general menu states
        
        public void InBeatmap() {
            songList.SetActive(false);
            mainMenu.SetActive(false);
            settingsMenu.SetActive(false);
            SetInGameMenusActive(true);
        }

        public void ToMainMenu() {
            songList.SetActive(false);
            mainMenu.SetActive(true);
            settingsMenu.SetActive(false);
            SetInGameMenusActive(false);
        }

        public void ToSettingsMenu() {
            songList.SetActive(false);
            mainMenu.SetActive(false);
            settingsMenu.SetActive(true);
            SetInGameMenusActive(false);
        }

        public void ToSongListMenu() {
            songList.SetActive(true);
            mainMenu.SetActive(false);
            settingsMenu.SetActive(false);
            SetInGameMenusActive(false);
            ListSongs(gm.songList.GetAllSongs());
        }
        
        //helper method for showing / hiding all different in game panels (including afterwards from mods added ones)
        private void SetInGameMenusActive(bool state) {
            foreach (var panel in inGamePanels) {
                panel.SetActive(state);
            }
        }

        // Quit App method for button call
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
            playBeatmapButton.onClick.AddListener(delegate { gm.StartBeatmap(song, song.difficulties[song.difficulties.Length-1], null); });
            deleteBeatmapButton.onClick.AddListener(delegate {  });
            practiceBeatmapButton.onClick.AddListener(delegate { gm.StartBeatmap(song, song.difficulties[0], null); });
            
            // set text
            beatmapTitleText.text = song.songName + " - " + song.songAuthorName + "\n" + song.songSubName;
            beatmapCoverImage.sprite = Util.LoadSprite(song.pathToDir + song.coverImageFile);
        }
        
        /* in-game panels */

        private void FindInGamePanels() {
            foreach (var panel in inGamePanels) {
                try {
                    scoreText = panel.transform.Find("Score").GetComponent<Text>();
                }
                catch (Exception e) {
                    Console.WriteLine(e);
                }
                try {
                    progressBar = panel.transform.Find("Progress.Bar").GetComponent<Image>();
                }
                catch (Exception e) {
                    Console.WriteLine(e);
                }
                try {
                    progrssText = panel.transform.Find("Progress.Text").GetComponent<Text>();
                }
                catch (Exception e) {
                    Console.WriteLine(e);
                }
                try {
                    multiplierText = panel.transform.Find("Multiplier.Text").GetComponent<Text>();
                }
                catch (Exception e) {
                    Console.WriteLine(e);
                }
            }
        }

        private void PopulateInGamePanels() {
            scoreText.text = gm.currentScore.ToString();
        }
        
        /* Build UI elements from code */

        public GameObject BuildUIElement(string text, UiType type, List<object> contents = null, int width = 0, int height = 0, int maxValue = 0, int minValue = 0) {
            GameObject output;
            switch (type) {
                case UiType.Button:
                    output = buttonPrefab;
                    output.GetComponentInChildren<Text>().text = text;
                    break;
                case UiType.Slider:
                    output = sliderPrefab;
                    var sldr = output.GetComponent<Slider>();
                    if (minValue != 0) {
                        sldr.minValue = minValue;
                    }
                    if (maxValue != 0) {
                        sldr.maxValue = maxValue;
                    }
                    break;
                case UiType.Color:
                    output = colorPrefab;
                    break;
                case UiType.Text:
                    output = textPrefab;
                    output.GetComponentInChildren<Text>().text = text;
                    break;
                case UiType.Int:
                    output = intPrefab;
                    output.GetComponentInChildren<Text>().text = text;
                    break;
                case UiType.Float:
                    output = floatPrefab;
                    output.GetComponentInChildren<Text>().text = text;
                    break;
                case UiType.Enum:
                    output = enumPrefab;
                    output.GetComponentInChildren<Text>().text = text;
                    break;
                case UiType.Category:
                    output = categoryPrefab;
                    output.GetComponentInChildren<Text>().text = text;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }

            return output;
        }
        
    }
}
