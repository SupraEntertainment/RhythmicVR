using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Valve.VR;
using Random = UnityEngine.Random;

namespace RhythmicVR {
    public class Core : MonoBehaviour {

        [Header("Prefabs")]
        public GameObject target;
        public GameObject obstacle;
    
        [Header("Materials")]
        public Material ambiguousMaterial;
        public Material leftMaterial;
        public Material rightMaterial;
        public Material centerMaterial;
        public static Material AMBIGUOUS_MAT;
        public static Material LEFT_MAT;
        public static Material RIGHT_MAT;
        public static Material CENTER_MAT;

        [Header("Tracking points")] 
        public Transform leftHand;
        public Transform rightHand;
        public Transform leftFoot;
        public Transform rightFoot;
        public Transform waist;

        [Header("Integrated settings menus")] 
        public SettingsField[] integratedSettings;

        public SettingsManager settingsManager;

        [Header("Other Properties")] 
        public float spawnDistance;
        public static float SPAWN_DISTANCE;
        public UiManager uiManager;
        [NonSerialized] public Config config;
        public PluginManager pluginManager;
        public SongList songList = new SongList();
        public SteamVR_Action_Boolean pauseButton;
        public SteamVR_Action_Pose pointerOffset;
        public SteamVR_Action_Pose gripPosition;
        public PluginBaseClass[] includedAssetPackages;
        public AudioSource audioSource;
        private List<GameObject> trackedObjects = new List<GameObject>();
        private List<GameObject> visualTrackedObjects = new List<GameObject>();
        private List<GameObject> menuTrackedObjects = new List<GameObject>();

        private bool allowPause;
        private bool isPaused;
        public bool songIsPlaying;
        
        public Beatmap currentlyPlayingBeatmap;
        public Song currentlyPlayingSong;
        public Gamemode currentGamemode;
        private TargetObject currentTargetObject;
        private GenericTrackedObject currentTrackedDeviceObject;
        private GameObject currentEnvironment;

        public float currentScore = 0;

        private void Start() {

            if (GetComponent<AudioSource>()) {
                audioSource = GetComponent<AudioSource>();
            } else {
                audioSource = new AudioSource();
            }
            settingsManager = new SettingsManager(this);
            pluginManager = new PluginManager(this);
            
            InitStaticVariables();

            InitConfig();

            LoadAssets();
            
            LoadSongsIntoSongList();

            InitializeSettings();

            //SetControllerPointerOffset();

            Util.FetchPoseOffset(OpenVR.System.GetTrackedDeviceIndexForControllerRole(ETrackedControllerRole.LeftHand), leftHand.Find("pointerOffset"), "tip"); 
            Util.FetchPoseOffset(OpenVR.System.GetTrackedDeviceIndexForControllerRole(ETrackedControllerRole.RightHand), rightHand.Find("pointerOffset"), "tip"); 
            
            uiManager.ListSongs(songList.GetAllSongs());

            //Util.FetchPoseOffset(OpenVR.System.GetTrackedDeviceIndexForControllerRole(ETrackedControllerRole.LeftHand), leftHand.Find("itemOffset"), "handGrip"); 
            //Util.FetchPoseOffset(OpenVR.System.GetTrackedDeviceIndexForControllerRole(ETrackedControllerRole.RightHand), rightHand.Find("itemOffset"), "handGrip"); 

            //RunATestSong();
        }

        private void SetControllerPointerOffset() {
            var l = leftHand.Find("pointerOffset");
            var r = rightHand.Find("pointerOffset");

            pointerOffset.UpdateTransform(SteamVR_Input_Sources.LeftHand, l);
            pointerOffset.UpdateTransform(SteamVR_Input_Sources.RightHand, r);
        }

        /// <summary>
        /// handle pause button
        /// </summary>
        private void Update() {
            if (allowPause) {
                if (pauseButton.stateUp) {
                    if (isPaused) {
                        ContinueBeatmap();
                        isPaused = false;
                    } else {
                        StopBeatmap(0);
                        isPaused = true;
                    }
                }
            }
        }

        /// <summary>
        /// Set all static values, this is a really jank way of doing and should never be done...
        /// Will replace it with something better eventually
        /// </summary>
        private void InitStaticVariables() {
            SPAWN_DISTANCE = spawnDistance;
        
            AMBIGUOUS_MAT = ambiguousMaterial;
            LEFT_MAT = leftMaterial;
            RIGHT_MAT = rightMaterial;
            CENTER_MAT = centerMaterial;
        }

        /// <summary>
        /// Initialize config object (assign if it can be loaded, otherwise create new and save it)
        /// </summary>
        private void InitConfig() {
            var logDir = Application.consoleLogPath.Substring(0, Application.consoleLogPath.Length - 10);
        
            // load config file / create if it doesn't exist already
            try {
                config = Config.Load(logDir);
                config.Save();
            }
            catch (Exception e) {
                Console.WriteLine(e);
                config = new Config();
                config.Save();
            }
            
        }

        /// <summary>
        /// Add included plugins to Plugin Manager, load first gamemode, set first environment
        /// </summary>
        private void LoadAssets() {
            foreach (var asset in includedAssetPackages) {
                pluginManager.AddPlugin(asset);
            }

            SetGamemode(pluginManager.GetAllGamemodes()[0]);
            uiManager.AddGamemodesToDropdown();
            SetEnvironment(pluginManager.GetAllEnvironments()[0]);
        }

        /// <summary>
        /// Initialize the Environment Prefab, delete previously loaded Environment if there is any
        /// </summary>
        /// <param name="go"></param>
        private void SetEnvironment(GameObject go) {
            if (currentEnvironment != null) {
                Destroy(currentEnvironment);
            }
            currentEnvironment = Instantiate(go);
        }

        /// <summary>
        /// Load all songs from the songPath (config object) into the songList
        /// </summary>
        private void LoadSongsIntoSongList() {
            List<Song> songs = new List<Song>();
            if (!Util.EnsureDirectoryIntegrity(config.songSavePath)) {
                return;
            }
            string[] paths = Directory.GetDirectories(config.songSavePath);
            foreach (var path in paths) {
                songs.Add(ReadSongFromPath(path + "/"));
            }
            songList.AddRange(songs);
        }

        /// <summary>
        /// Add all integrated settings to Settings manager (UI) and add listeners to inputs
        /// </summary>
        private void InitializeSettings() {
            // add listeners
            /*for (var i = 0; i < integratedSettings.Length; i++) {
                var setting = integratedSettings[i];
                switch (i) {
                    case 0:
                        setting.call.AddListener(delegate {
                            volumeManager.SetGeneralVolume(int.Parse(setting._input));
                        });
                        break;
                    case 1:
                        setting.call.AddListener(delegate {
                            volumeManager.SetMenuVolume(int.Parse(setting._input));
                        });
                        break;
                    case 2:
                        setting.call.AddListener(delegate {
                            volumeManager.SetSongVolume(int.Parse(setting._input));
                        });
                        break;
                    case 3:
                        setting.call.AddListener(delegate {
                            volumeManager.SetSongPreviewVolume(int.Parse(setting._input));
                        });
                        break;
                    case 4:
                        setting.call.AddListener(delegate {
                            volumeManager.SetHitVolume(int.Parse(setting._input));
                        });
                        break;
                    case 5:
                        setting.call.AddListener(delegate {
                            volumeManager.SetMissVolume(int.Parse(setting._input));
                        });
                        break;
                    case 6:
                        setting.call.AddListener(delegate {
                            volumeManager.SetWrongHitVolume(int.Parse(setting._input));
                        });
                        break;
                }
            }*/

            settingsManager.settings.AddRange(integratedSettings);
            settingsManager.settingsMenuParent = uiManager.settingsMenu;
            settingsManager.UpdateSettingsUi();
        }

        /// <summary>
        /// Create a fake beatmap with some test notes and play them
        /// </summary>
        private void RunATestSong() {
            // create debug Beatmap
            var bm = new Beatmap();
            var possibleTypes = new List<TrackingPoint[]> {
                new [] {TrackingPoint.LeftFoot},
                new [] {TrackingPoint.LeftHand},
                new [] {TrackingPoint.Waist},
                new [] {TrackingPoint.LeftFoot},
                new [] {TrackingPoint.LeftFoot, TrackingPoint.LeftHand, TrackingPoint.RightHand, TrackingPoint.RightFoot, TrackingPoint.Waist},
                new [] {TrackingPoint.RightFoot}
            };
            var notes = new List<Note>();
            for (var i = 0; i < 240; i++) {
                notes.Add(new Note {
                    time = Random.Range((i - 0.5f) / 4f, (i + 0.5f) / 4f),
                    cutDirection = i * 6,
                    type = possibleTypes[(int) Math.Floor(Random.value * 6)],
                    xPos = Random.value * 2 - 1,
                    yPos = Random.value * 2
                });
            }

            bm.notes = notes.ToArray();
            // play beatmap
            StartCoroutine(PlayNotes(bm));
        }

        /// <summary>
        /// Load a Song from path and add to songList, reload songList UI
        /// </summary>
        /// <param name="songPath">The Folder path, where level.json is stored in</param>
        public void LoadSong(string songPath) {
            var song = ReadSongFromPath(songPath);
            Debug.Log("Loaded Song " + song.songName + " by " + song.songAuthorName);
            songList.Add(song);
            uiManager.ListSongs(songList.GetAllSongs());
            //directly play the first beatmap from that song
            //Debug.Log("Playing Beatmap " + song.difficulties[0].name + " by " + song.difficulties[0].beatMapAuthor);
            //StartBeatmap(song, song.difficulties[0]);
        }

        /// <summary>
        /// Read a song from path to song object
        /// </summary>
        /// <param name="songPath">The Folder path, where level.json is stored in</param>
        /// <returns>The Song object</returns>
        private static Song ReadSongFromPath(string songPath) {
            var song = JsonUtility.FromJson<Song>(File.ReadAllText(songPath + "level.json"));
            song.pathToDir = songPath;
            return song;
        }

        /// <summary>
        /// Start a Beatmap
        /// </summary>
        /// <param name="song">The Song to play</param>
        /// <param name="difficulty">The Difficulty, of which to take the beatmap</param>
        /// <param name="modfiers">[Not Implemented] The modifiers to use while playing</param>
        public void StartBeatmap(Song song, Difficulty difficulty, string modfiers) {
            Time.timeScale = 0;
            var bm = JsonUtility.FromJson<Beatmap>(File.ReadAllText(song.pathToDir + "/" + difficulty.beatMapPath));
            uiManager.InBeatmap();
            songIsPlaying = true;
            allowPause = true;
            currentlyPlayingBeatmap = bm;
            currentlyPlayingSong = song;
            ((ScoreManager.ScoreManager) pluginManager.Find("Score Manager")).SelectSongAndAddPlaythrough(song);
            //StopCoroutine(PlayBeatmap(bm));
            StartCoroutine(PlayNotes(bm));
            PlaySongAudio(song);
        }

        /// <summary>
        /// Play the audio belonging to song object
        /// </summary>
        /// <param name="song">The song, who's audio to play</param>
        private void PlaySongAudio(Song song) {
            var path = song.pathToDir + song.songFile;
            audioSource.clip = Util.GetAudioClipFromPath(path);
            audioSource.Play();
            Time.timeScale = 1;
        }

        /// <summary>
        /// Play Notes coroutine, plays all notes timed properly
        /// </summary>
        /// <param name="bm">The beatmap to take the notes from</param>
        /// <returns></returns>
        private IEnumerator PlayNotes(Beatmap bm) {
            //var beatmapLength = bm.notes[bm.notes.Length-1].time;
            float currentTime = 0;
            for (var i = 0; i < bm.notes.Length; i++) {
                var note = bm.notes[i];
                Debug.Log("Wait time: " + (note.time - currentTime));
                yield return new WaitForSeconds((note.time - currentTime));
                currentTime = note.time;
                Debug.Log("Current Time: " + currentTime);
                SpawnTarget(note.speed, note.xPos, note.yPos, note.cutDirection, note.rotation, note.type);
            }

            Debug.Log("Finished placing target objects");
            StopBeatmap(1);
        }

        /// <summary>
        /// Pauses the currently playing beatmap.
        /// </summary>
        /// <param name="reason">
        /// 0 = paused<br/>
        /// 1 = completed<br/>
        /// 2 = failed</param>
        public void StopBeatmap(int reason) {
            songIsPlaying = false;
            audioSource.Pause();
            switch (reason) {
                case 0: // paused
                    Time.timeScale = 0;
                    uiManager.ShowPauseMenu(reason);
                    break;
                case 1: // completed
                    uiManager.ShowPauseMenu(reason);
                    break;
                case 2: // failed
                    uiManager.ShowPauseMenu(reason);
                    break;
            }
        }

        /// <summary>
        /// Continue the paused beatmap. Hides Pause menu and sets the time scale back to 1 in order to continue.
        /// </summary>
        public void ContinueBeatmap() {
            songIsPlaying = true;
            audioSource.Play();
            Time.timeScale = 1;
            uiManager.HidePauseMenu();
            isPaused = false;
        }

        /// <summary>
        /// Exit the currently playing beatmap (stops the coroutine, disables pause button, hides pause menu, and returns to the menu)
        /// </summary>
        public void ExitBeatmap() {
            StopCoroutine(PlayNotes(currentlyPlayingBeatmap));
            ((ScoreManager.ScoreManager) pluginManager.Find("Score Manager")).SaveCurrentScores();
            audioSource.time = 0;
            audioSource.clip = null;
            allowPause = false;
            uiManager.ToSongListMenu();
            uiManager.HidePauseMenu();
            isPaused = false;
        }

        /// <summary>
        /// Write song, all Beatmaps, the Cover Image and Audio to their Files
        /// </summary>
        /// <param name="songObject">The "Song" Object</param>
        /// <param name="beatmaps">The "Beatmap" Objects</param>
        /// <param name="cover">The cover Image as byte Array</param>
        /// <param name="audio">The audio File as byte Array</param>
        /// <returns>the path to the song</returns>
        public string SaveSongToFile(Song songObject, Beatmap[] beatmaps, Byte[] cover, Byte[] audio) {
            string pathToSong = config.songSavePath + songObject.id + "_" + songObject.songName.Replace("/", "") + "/";
            if (!Directory.Exists(pathToSong)) {
                Directory.CreateDirectory(pathToSong);
            }
            File.WriteAllText(pathToSong + "level.json", JsonUtility.ToJson(songObject, true));
            File.WriteAllBytes(pathToSong + songObject.coverImageFile, cover);
            File.WriteAllBytes(pathToSong + songObject.songFile, audio);
            for (var index = 0; index < beatmaps.Length; index++) {
                var beatmap = beatmaps[index];
                File.WriteAllText(pathToSong + songObject.difficulties[index].beatMapPath, JsonUtility.ToJson(beatmap, true));
            }

            return pathToSong;
        }
        
        /// <summary>
        /// Loads a Gamemode (sets appropriate target object and tracked Objects (colliders and hit logic) to Tracking points
        /// </summary>
        /// <param name="gm">The gamemode to load</param>
        public void SetGamemode(Gamemode gm) {
            foreach (var trackedObject in trackedObjects) {
                Destroy(trackedObject);
            }
            foreach (var trackedObject in visualTrackedObjects) {
                Destroy(trackedObject);
            }
            currentGamemode = gm;
            target = gm.targetObject; //set the target object
            currentTargetObject = target.GetComponent<TargetObject>(); //set the target component (TODO: still arguing about which of theese two to use)
            trackedObjects.Clear();
            visualTrackedObjects.Clear();
            
            
            foreach (var trackedDevicePair in gm.trackedObjects) { // iterate over all available tracking points in gamemode
                // unnecessary and bad: trackedDevicePair.prefab.GetComponent<GenericTrackedObject>().collider.gameObject.tag = TrackerRoleToTag(trackedDevicePair.role); // set tag for determining position
                Transform tracker;
                switch (trackedDevicePair.role) { // set tracked objects (colliders and hit logic) to tracking points 
                    case TrackingPoint.LeftHand:
                        tracker = leftHand;
                        break;
                    case TrackingPoint.RightHand:
                        tracker = rightHand;
                        break;
                    case TrackingPoint.LeftFoot:
                        tracker = leftFoot;
                        break;
                    case TrackingPoint.RightFoot:
                        tracker = rightFoot;
                        break;
                    case TrackingPoint.Waist:
                        tracker = waist;
                        break;
                    default:
                        return;
                }
                trackedObjects.Add(Instantiate(trackedDevicePair.prefab, tracker.Find("itemOffset")));
                visualTrackedObjects.Add(Instantiate(trackedDevicePair.defaultVisualPrefab, tracker.Find("itemOffset"))); //TODO use selected prefab for gamemode, only fallback if empty
            }
        }
    
        /// <summary>
        /// Spawn a Target Object
        /// </summary>
        /// <param name="speed">The speed, in wich that note travels</param>
        /// <param name="xCoord">The x position</param>
        /// <param name="yCoord">The y position</param>
        /// <param name="viewRotation">The Rotation along the notes travel direction</param>
        /// <param name="playspaceRoation">The notes rotation along the gamemode origin</param>
        /// <param name="hand">The Tracking points the note is supposed to be hit with</param>
        public void SpawnTarget(float speed, float xCoord, float yCoord, float viewRotation, float playspaceRoation, TrackingPoint[] hand) {
            GameObject cube = Instantiate(currentTargetObject.gameObject, new Vector3(xCoord, yCoord, SPAWN_DISTANCE), new Quaternion(0, 0, viewRotation, 0));
            cube.GetComponent<TargetObject>().InitNote(new Note(1, xCoord, yCoord, speed, hand, viewRotation, playspaceRoation));
        }

        // spawn a obstacle ^ same here
        public void SpawnObstacle(float speed, float xCoord, float yCoord, float viewRotation, float playspaceRoation, float width, float height) {
        
        }
    }
}