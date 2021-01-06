using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using BeatSaber;
using UnityEngine;
using UnityEngine.XR.Management;
//using Valve.VR;

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

        [Header("Other Properties")] 
        public Transform playerTransform;
        public float spawnDistance;
        public UiManager uiManager;
        [NonSerialized] public Config config;
        public PluginManager pluginManager;
        public SongList songList = new SongList();
        //public SteamVR_Action_Boolean pauseButton;
        //public SteamVR_Action_Pose pointerOffset;
        //public SteamVR_Action_Pose gripPosition;
        [NonSerialized]  public SettingsManager settingsManager;
        [NonSerialized] public AudioSource audioSource;
        private List<GameObject> trackedObjects = new List<GameObject>();
        private List<GameObject> visualTrackedObjects = new List<GameObject>();
        private List<GameObject> menuTrackedObjects = new List<GameObject>();

        [NonSerialized] public bool allowPause;
        [NonSerialized] public bool isPaused;
        [NonSerialized] public bool songIsPlaying;

        [NonSerialized] public string playerName = "No Player Name";
        
        [NonSerialized] public Song currentlyPlayingSong;
        [NonSerialized] public Gamemode currentGamemode;
        [NonSerialized] public TargetObject currentTargetObject;
        [NonSerialized] public GenericTrackedObject currentTrackedDeviceObject;
        [NonSerialized] public GameObject currentEnvironment;
        private BeatSaberImportPlugin bsip;

        public float currentScore = 0;

        private void Start() {
	        XRGeneralSettings settings = XRGeneralSettings.Instance;
	        settings.Manager.activeLoader.Start();

            if (GetComponent<AudioSource>()) {
                audioSource = GetComponent<AudioSource>();
            } else {
                audioSource = new AudioSource();
            }
            settingsManager = new SettingsManager(this);
            pluginManager = new PluginManager(this);
            
            InitStaticVariables();

            InitConfig();

            if (config.useSteamUsername) {
                playerName = config.steamUsername;
            } else {
                playerName = config.localUsername;
            }

            LoadPlugins();
            
            bsip = (BeatSaberImportPlugin)pluginManager.Find("Beat Saber Import");
            
            LoadSongsIntoSongList();

            InitializeSettings();

            //SetControllerPointerOffset();

            //Util.FetchPoseOffset(OpenVR.System.GetTrackedDeviceIndexForControllerRole(ETrackedControllerRole.LeftHand), leftHand.Find("pointerOffset"), "tip"); 
            //Util.FetchPoseOffset(OpenVR.System.GetTrackedDeviceIndexForControllerRole(ETrackedControllerRole.RightHand), rightHand.Find("pointerOffset"), "tip"); 
            
            uiManager.ListSongs(songList.GetAllSongs());

            //Util.FetchPoseOffset(OpenVR.System.GetTrackedDeviceIndexForControllerRole(ETrackedControllerRole.LeftHand), leftHand.Find("itemOffset"), "handGrip"); 
            //Util.FetchPoseOffset(OpenVR.System.GetTrackedDeviceIndexForControllerRole(ETrackedControllerRole.RightHand), rightHand.Find("itemOffset"), "handGrip"); 

            //RunATestSong();
        }

        private void SetPlayerOffsets() {
            playerTransform.position = new Vector3(config.playspacePosition[0], config.playspacePosition[1], config.playspacePosition[2]);
            playerTransform.rotation = Quaternion.Euler(config.playspaceRotation[0], config.playspaceRotation[1], config.playspaceRotation[2]);
        }

        /*private void SetControllerPointerOffset() {
            var l = leftHand.Find("pointerOffset");
            var r = rightHand.Find("pointerOffset");

            pointerOffset.UpdateTransform(SteamVR_Input_Sources.LeftHand, l);
            pointerOffset.UpdateTransform(SteamVR_Input_Sources.RightHand, r);
        }*/

        /// <summary>
        /// handle pause button
        /// </summary>
        /*private void Update() {
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
        }*/

        /// <summary>
        /// Set all static values, this is a really jank way of doing and should never be done...
        /// Will replace it with something better eventually
        /// </summary>
        private void InitStaticVariables() {
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
        private void LoadPlugins() {
            //internal plugins
            pluginManager.LoadPluginsFromFolder(Application.streamingAssetsPath + "/plugins/");
            
            //external plugins
            pluginManager.LoadPluginsFromFolder(config.pluginSavePath);

            if (pluginManager.GetAllGamemodes().Count != 0) {
                SetGamemode(pluginManager.GetAllGamemodes()[0]);
            }
            if (pluginManager.GetAllEnvironments().Count != 0) {
                SetEnvironment(pluginManager.GetAllEnvironments()[0]);
            }
            
            uiManager.AddGamemodesToDropdown();
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
        public void LoadSongsIntoSongList() {
            songList.Clear();
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
            List<SettingsField> coreSettings = new List<SettingsField>();

            {
                SettingsField setting = new SettingsField("Song save Path", UiType.Text, uiManager.textPrefab, "Settings/Files", config.songSavePath);
                setting.stringCall = delegate(string arg0) { config.songSavePath = arg0; };
                coreSettings.Add(setting);
            }

            {
                SettingsField setting = new SettingsField("Plugin save Path", UiType.Text, uiManager.textPrefab, "Settings/Files", config.pluginSavePath);
                setting.stringCall = delegate(string arg0) { config.pluginSavePath = arg0; };
                coreSettings.Add(setting);
            }

            {
                SettingsField setting = new SettingsField("keyboard config Path", UiType.Text, uiManager.textPrefab, "Settings/Files", config.keyboardSavePath);
                setting.stringCall = delegate(string arg0) { config.keyboardSavePath = arg0; };
                coreSettings.Add(setting);
            }

            {
                SettingsField setting = new SettingsField("Use Steam Username", UiType.Bool, uiManager.boolPrefab, "Settings/Player", config.useSteamUsername);
                setting.boolCall = delegate(bool arg0) { config.useSteamUsername = arg0; };
                coreSettings.Add(setting);
            }

            {
                SettingsField setting = new SettingsField("Local Username", UiType.Text, uiManager.textPrefab, "Settings/Player", config.localUsername);
                setting.stringCall = delegate(string arg0) { config.localUsername = arg0; };
                coreSettings.Add(setting);
            }

            {
                SettingsField setting = new SettingsField("Super Sampling", UiType.Float, uiManager.floatPrefab, "Settings/Video", config.superSampling);
                setting.floatCall = delegate(float arg0) { config.superSampling = arg0; };
                setting.maxValue = 5;
                setting.minValue = 0.2f;
                coreSettings.Add(setting);
            }

            {
                SettingsField setting = new SettingsField("Camera Smoothing (desktop)", UiType.Int, uiManager.intPrefab, "Settings/Video", config.cameraSmoothing);
                setting.floatCall = delegate(float arg0) { config.cameraSmoothing = (int)arg0; };
                setting.maxValue = 10;
                setting.minValue = 0;
                coreSettings.Add(setting);
            }

            {
                Vector3 initialValue = new Vector3(config.playspacePosition[0], config.playspacePosition[1], config.playspacePosition[2]);
                SettingsField setting = new SettingsField("Position", UiType.Vector3, uiManager.vector3Prefab, "Settings/Offsets/Playspace", initialValue);
                setting.vectorNCall = delegate(int arg0, float arg1) { config.playspacePosition[arg0] = arg1; };
                coreSettings.Add(setting);
            }

            {
                Vector3 initialValue = new Vector3(config.playspaceRotation[0], config.playspaceRotation[1], config.playspaceRotation[2]);
                SettingsField setting = new SettingsField("Rotation", UiType.Vector3, uiManager.vector3Prefab, "Settings/Offsets/Playspace", initialValue);
                setting.vectorNCall = delegate(int arg0, float arg1) { config.playspaceRotation[arg0] = arg1; };
                coreSettings.Add(setting);
            }

            {
                SettingsField setting = new SettingsField("Reset", UiType.Button, uiManager.buttonPrefab, "Settings/Offsets/Playspace");
                setting.buttonCall = delegate() {
                    config.playspacePosition = new float[]{0,0,0};
                    config.playspaceRotation = new float[]{0,0,0};
                    setting.page.elementChildren[0].SetValues(new Vector3(0,0,0));
                    setting.page.elementChildren[1].SetValues(new Vector3(0,0,0));
                };
                coreSettings.Add(setting);
            }

            {
                Vector3 initialValue = new Vector3(config.controllerPosition[0], config.controllerPosition[1], config.controllerPosition[2]);
                SettingsField setting = new SettingsField("Position", UiType.Vector3, uiManager.vector3Prefab, "Settings/Offsets/Controller", initialValue);
                setting.vectorNCall = delegate(int arg0, float arg1) { config.controllerPosition[arg0] = arg1; };
                coreSettings.Add(setting);
            }

            {
                Vector3 initialValue = new Vector3(config.controllerRotation[0], config.controllerRotation[1], config.controllerRotation[2]);
                SettingsField setting = new SettingsField("Rotation", UiType.Vector3, uiManager.vector3Prefab, "Settings/Offsets/Controller", initialValue);
                setting.vectorNCall = delegate(int arg0, float arg1) { config.controllerRotation[arg0] = arg1; };
                coreSettings.Add(setting);
            }

            {
                SettingsField setting = new SettingsField("Reset", UiType.Button, uiManager.buttonPrefab, "Settings/Offsets/Controller");
                setting.buttonCall = delegate() {
                    config.controllerPosition = new float[]{0,0,0};
                    config.controllerRotation = new float[]{0,0,0};
                    setting.page.elementChildren[0].SetValues(new Vector3(0,0,0));
                    setting.page.elementChildren[1].SetValues(new Vector3(0,0,0));
                };
                coreSettings.Add(setting);
            }

            
            settingsManager.settings.AddRange(coreSettings);
            settingsManager.settingsMenuParent = uiManager.settingsMenu;
            settingsManager.UpdateSettingsUi();
        }

        /*
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
                    target = possibleTypes[(int) Math.Floor(Random.value * 6)],
                    xPos = Random.value * 2 - 1,
                    yPos = Random.value * 2
                });
            }

            bm.notes = notes.ToArray();
            // play beatmap
            StartCoroutine(PlayNotes(bm));
        }*/

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

        public void ExitBeatmap() {
            currentGamemode.ExitBeatmap();
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
        /// Write song, all Beatmaps, the Cover Image and Audio to their Files
        /// </summary>
        /// <param name="songObject">The "Song" Object</param>
        /// <param name="beatmaps">The "Beatmap" Objects</param>
        /// <param name="coverData">The cover Image as byte Array</param>
        /// <param name="audioData">The audio File as byte Array</param>
        /// <param name="coverFilePath">The file path to the cover image file to copy to the target directory</param>
        /// <param name="audioFilePath">The file path to the audio file to copy to the target directory</param>
        /// <returns>the path to the song</returns>
        public string SaveSongToFile<T>(Song songObject, T[] beatmaps, byte[] coverData = null, byte[] audioData = null, string coverFilePath = "", string audioFilePath = "") {
            string songName = songObject.songName;
            foreach (var invalidCharacter in Path.GetInvalidPathChars()) {
                songName = songName.Replace(invalidCharacter, '_');
            }
            string pathToSong = config.songSavePath + songObject.id + "_" + songName + "/";
            
            if (!Directory.Exists(pathToSong)) {
                Directory.CreateDirectory(pathToSong);
            }
            File.WriteAllText(pathToSong + "level.json", JsonUtility.ToJson(songObject, true));

            // copy file or write from bytes
            if (coverFilePath != null && !File.Exists(pathToSong + songObject.coverImageFile)) {
                File.Copy(coverFilePath, pathToSong + songObject.coverImageFile);
            }
            else if (coverData != null) {
                File.WriteAllBytes(pathToSong + songObject.coverImageFile, coverData);
            }
            
            // copy file or write from bytes
            if (audioFilePath != null && !File.Exists(pathToSong + songObject.songFile)) {
                File.Copy(audioFilePath, pathToSong + songObject.songFile);
            }
            else if (audioData != null) {
                File.WriteAllBytes(pathToSong + songObject.songFile, audioData);
            }

            for (var i = 0; i < beatmaps.Length; i++) {
                var beatmap = beatmaps[i];
                File.WriteAllText(pathToSong + songObject.difficulties[i].beatMapPath, JsonUtility.ToJson(beatmap, true));
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
            trackedObjects.Clear();
            
            foreach (var trackedObject in visualTrackedObjects) {
                Destroy(trackedObject);
            }
            visualTrackedObjects.Clear();
            
            currentGamemode = gm;
            target = gm.targetObject; //set the target object
            currentTargetObject = target.GetComponent<TargetObject>(); //set the target component (TODO: still arguing about which of these two to use)
            
            
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

                var to = Instantiate(trackedDevicePair.prefab, tracker.Find("itemOffset"));
                to.GetComponent<GenericTrackedObject>().role = trackedDevicePair.role;
                trackedObjects.Add(to);
                visualTrackedObjects.Add(Instantiate(trackedDevicePair.defaultVisualPrefab, tracker.Find("itemOffset"))); //TODO use selected prefab for gamemode, only fallback if empty
            }
        }

        public GameObject SimpleInstantiate(GameObject prefab) {
            return Instantiate(prefab);
        }
        
    }
}