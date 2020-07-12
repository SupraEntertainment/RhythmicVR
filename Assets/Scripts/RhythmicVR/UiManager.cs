using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using SFB;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Valve.VR;

namespace RhythmicVR {
    public class UiManager : MonoBehaviour {

        public Core core;
        public RectTransform songListParent;
        public SteamVR_ActionSet uiActionSet;

        [Header("Menus")] 
        public GameObject songList;
        public GameObject mainMenu;
        public GameObject settingsMenu;
        public GameObject pauseMenu;
        public GameObject titleText;
        public GameObject inGame;

        [Header("Song Info Panel elements")]
        public Button playBeatmapButton;
        public Button deleteBeatmapButton;
        public Button practiceBeatmapButton;
        public Text beatmapTitleText;
        public Image beatmapCoverImage;
        public Transform songPropertyPanel;
        public Transform beatmapCategoryPanel;
        public Transform beatmapDifficultyPanel;
        public GameObject difficultyButton;
        public GameObject propertyPanel;

        [Header("In Game Panels")] 
        public Text scoreText;
        public Text progressText;
        public SlicedFilledImage progressBar;
        public Text multiplierText;
        
        [Header("UI Prefabs")]
        public GameObject songListItem;
        public GameObject scrollList;
        public GameObject buttonPrefab;
        public GameObject vector3Prefab;
        public GameObject intPrefab;
        public GameObject floatPrefab;
        public GameObject textPrefab;
        public GameObject colorPrefab;
        public GameObject boolPrefab;
        public GameObject categoryPrefab;
        public GameObject enumPrefab;

        [Header("Various")] 
        public Dropdown gamemodeDropdown;

        public Dictionary<string, object> displayedProperties = new Dictionary<string, object>();

        private ScoreManager.ScoreManager scoreManager;
        private List<GameObject> loadedSongs = new List<GameObject>();

        public void Start() {
            scoreManager = (ScoreManager.ScoreManager) core.pluginManager.Find("Score Manager");
            uiActionSet.Activate();
            ToMainMenu();
            PopulateInGamePanels();
        }

        public void Update() {
            if (core.songIsPlaying) {
                PopulateInGamePanels();
            }
        }

        // song file loading, should belong into its own plugin
    
        public void LoadSong() {
            new Thread(OpenSongFile).Start();
        }
    
        public void OpenSongFile() {
            StandaloneFileBrowser.OpenFolderPanelAsync("Load and Play Beatmap", "", true, delegate(string[] strings) {
                foreach (var path in strings) {
                    core.LoadSong(path + Path.DirectorySeparatorChar);
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
        
        /// <summary>
        /// Go in game, sets all ingame menus (score, etc) active, disables all other UI
        /// </summary>
        public void InBeatmap() {
            songList.SetActive(false);
            mainMenu.SetActive(false);
            settingsMenu.SetActive(false);
            inGame.SetActive(true);
            titleText.SetActive(false);
        }

        /// <summary>
        /// Go to main menu
        /// </summary>
        public void ToMainMenu() {
            songList.SetActive(false);
            mainMenu.SetActive(true);
            settingsMenu.SetActive(false);
            inGame.SetActive(false);
            titleText.SetActive(true);
        }

        /// <summary>
        /// Go to settings menu
        /// </summary>
        public void ToSettingsMenu() {
            songList.SetActive(false);
            mainMenu.SetActive(false);
            settingsMenu.SetActive(true);
            inGame.SetActive(false);
            titleText.SetActive(true);
        }

        /// <summary>
        /// Go to song list menu, also refreshes song list
        /// </summary>
        public void ToSongListMenu() {
            songList.SetActive(true);
            mainMenu.SetActive(false);
            settingsMenu.SetActive(false);
            inGame.SetActive(false);
            titleText.SetActive(true);
            ListSongs(core.songList.GetAllSongs());
        }

        /// <summary>
        /// Quit App method for button call
        /// </summary>
        public void Quit() {

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
            Application.Quit();
        }

        /// <summary>
        /// Select gamemode from dropdown
        /// </summary>
        /// <param name="option">the selected option in dropdown (should be sorted similar to list in PluginManager)</param>
        public void SelectGamemode(int option) {
            core.SetGamemode(core.pluginManager.GetAllGamemodes()[option]);
        }

        /// <summary>
        /// Adds all gamemodes listed in pluginManager to dropdown on songList
        /// </summary>
        public void AddGamemodesToDropdown() {
            foreach (var gamemode in core.pluginManager.GetAllGamemodes()) {
                gamemodeDropdown.options.Add(new Dropdown.OptionData(gamemode.gamemodeName));
            }
        }
        
        /*-------------------- vv songlist vv --------------------------*/

        public void ListSongs(List<Song> songs) {
            ListAllSongs(songListParent, songs, songListItem);
        }

        /// <summary>
        /// List songs given in UI song list
        /// </summary>
        /// <param name="parent">The parent object to put the songs into</param>
        /// <param name="songs">The songs</param>
        /// <param name="buttonPrefab">The Prefab to use as song List element</param>
        private void ListAllSongs(Transform parent, IReadOnlyList<Song> songs, GameObject buttonPrefab) {
            if (loadedSongs.Count != 0) {
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
                loadedSongs.Add(button);
            }
        }

        private void RemoveAllListedSongs() {
            foreach (var loadedSong in loadedSongs) {
                Destroy(loadedSong);
            }
        }
        
        /// <summary>
        /// Display song info
        /// </summary>
        /// <param name="song">The song to display info about</param>
        public void DisplaySongInfo(Song song) {
            Debug.Log("Displaying Song " + song.songName + " by " + song.songAuthorName);

            core.currentlyPlayingSong = song;
            
            // remove listeners from previous song
            playBeatmapButton.onClick.RemoveAllListeners();
            deleteBeatmapButton.onClick.RemoveAllListeners();
            practiceBeatmapButton.onClick.RemoveAllListeners();
            
            // add listeners
            playBeatmapButton.onClick.AddListener(delegate { core.StartBeatmap(song, song.difficulties[song.difficulties.Length-1], null); });
            deleteBeatmapButton.onClick.AddListener(delegate {  });
            practiceBeatmapButton.onClick.AddListener(delegate { core.StartBeatmap(song, song.difficulties[0], null); });
            
            // set text
            beatmapTitleText.text = song.songName + " - " + song.songAuthorName + "\n" + song.songSubName;
            beatmapCoverImage.sprite = Util.LoadSprite(song.pathToDir + song.coverImageFile);

            for (int i = 0; i < beatmapDifficultyPanel.transform.childCount; i++) {
                Destroy(beatmapDifficultyPanel.transform.GetChild(i).gameObject);
            }

            var difButHeight = beatmapDifficultyPanel.GetComponent<RectTransform>().sizeDelta.y / (song.difficulties.Length);
            for (var i = 0; i < song.difficulties.Length; i++) {
                var i1 = i;
                var difficulty = song.difficulties[i];
                var button = Instantiate(difficultyButton, beatmapDifficultyPanel);
                button.GetComponentInChildren<Text>().text = song.difficulties[i].name;
                button.GetComponent<Button>().onClick.AddListener(delegate {
                    SetPlayButtonListener(delegate {
                        core.StartBeatmap(song, song.difficulties[i1], null);
                    });
                    Debug.Log("Selected Difficulty " + i1);
                });
                var rt = button.GetComponent<RectTransform>();
                rt.anchoredPosition = new Vector2(rt.anchoredPosition.x, -i * difButHeight);
                button.GetComponent<RectTransform>().sizeDelta = new Vector2(rt.sizeDelta.x, difButHeight);
            }
            
            displayedProperties.Clear();
            displayedProperties.Add("bpm", song.beatsPerMinute);

            SetDisplayedProperties();
        }

        public void SetDisplayedProperties() {
            for (int i = 0; i < songPropertyPanel.transform.childCount; i++) {
                Destroy(songPropertyPanel.transform.GetChild(i).gameObject);
            }
            
            var propButWidth = songPropertyPanel.GetComponent<RectTransform>().sizeDelta.x / (displayedProperties.Count);
            for (var i = 0; i < displayedProperties.Count; i++) {
                var button = Instantiate(propertyPanel, songPropertyPanel);
                button.GetComponentInChildren<Text>().text = displayedProperties.Keys.ElementAt(i) + "\n" + displayedProperties.Values.ElementAt(i).ToString();
                var rt = button.GetComponent<RectTransform>();
                rt.anchoredPosition = new Vector2(-i * propButWidth, rt.anchoredPosition.y);
                button.GetComponent<RectTransform>().sizeDelta = new Vector2(propButWidth, rt.sizeDelta.y);
            }
        }

        public void SetPlayButtonListener(UnityAction call) {
            playBeatmapButton.onClick.RemoveAllListeners();
            playBeatmapButton.onClick.AddListener(call);
        }
        
        /* in-game panels */

        private void PopulateInGamePanels() {
            if (scoreManager.scoreConfig.showOverlay) {
                if (scoreManager.scoreConfig.showOverallScore) {
                    scoreText.text = scoreManager.currentPlaythrough.GetScore().ToString();
                }
                if (scoreManager.scoreConfig.showMultiplier) {
                    multiplierText.text = scoreManager.currentPlaythrough.GetMultiplier().x.ToString();
                }
                if (scoreManager.scoreConfig.showCurrentTargetScore) {
                    //scoreText.text = scoreManager.currentPlaythrough.GetScore().ToString();
                }

                if ((object)core.audioSource.clip != null) {
                    var audioSource = core.audioSource;
                    progressBar.fillAmount = 1 / audioSource.clip.length * audioSource.time;
                    progressText.text = Util.ParseSeconds(audioSource.time) + " / " + Util.ParseSeconds(audioSource.clip.length);
                }
            }
        }

        /// <summary>
        /// Build UI elements from code (via prefabs)
        /// </summary>
        /// <param name="text">The base text to use</param>
        /// <param name="type">Wich type of UiElement to generate</param>
        /// <param name="contents">If it has multiple conents (e.g. enum / dropdown)</param>
        /// <param name="maxValue">for sliders</param>
        /// <param name="minValue">for sliders</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public GameObject BuildUiElement(string text, UiType type, List<object> contents = null, int maxValue = 0, int minValue = 0) {
            GameObject output;
            switch (type) {
                case UiType.Button:
                    output = buttonPrefab;
                    output.GetComponentInChildren<Text>().text = text;
                    break;
                case UiType.Vector3:
                    output = vector3Prefab;
                    output.GetComponentInChildren<Text>().text = text;
                    break;
                case UiType.Color:
                    output = colorPrefab;
                    output.GetComponentInChildren<Text>().text = text;
                    break;
                case UiType.Text:
                    output = textPrefab;
                    output.GetComponentInChildren<Text>().text = text;
                    break;
                case UiType.Int:
                    output = intPrefab;
                    SetupSlider(output, minValue, maxValue);
                    output.GetComponentInChildren<Text>().text = text;
                    break;
                case UiType.Float:
                    output = floatPrefab;
                    SetupSlider(output, minValue, maxValue);
                    output.GetComponentInChildren<Text>().text = text;
                    break;
                case UiType.Enum:
                    output = enumPrefab;
                    output.GetComponentInChildren<Text>().text = text;
                    break;
                case UiType.Bool:
                    output = boolPrefab;
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

        private void SetupSlider(GameObject slider, int minValue, int maxValue) {
            var sldr = slider.GetComponent<Slider>();
            if (minValue != 0) {
                sldr.minValue = minValue;
            }
            if (maxValue != 0) {
                sldr.maxValue = maxValue;
            }
        }
    }
}
