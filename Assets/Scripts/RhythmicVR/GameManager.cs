using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using Valve.VR;
using Random = UnityEngine.Random;

namespace RhythmicVR {
    public class GameManager : MonoBehaviour {

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

        private SettingsManager settingsManager;

        [Header("Other Properties")] 
        public float spawnDistance;
        public static float SPAWN_DISTANCE;
        public UIManager uiManager;
        [NonSerialized] public Config config;
        public PluginManager pluginManager;
        public SongList songList = new SongList();
        public SteamVR_Action_Boolean pauseButton;
        public AssetPackage[] includedAssetPackages;

        private bool allowPause;
        private bool isPaused;
        
        [NonSerialized] public Beatmap currentlyPlayingBeatmap;
        [NonSerialized] private Gamemode _currentGamemode;
        [NonSerialized] private TargetObject _currentTargetObject;
        [NonSerialized] private GenericTrackedObject _currentTrackedDeviceObject;
        [NonSerialized] private GameObject _currentEnvironment;

        [NonSerialized] public float currentScore = 0;

        private void Start() {

            settingsManager = new SettingsManager(this);
            pluginManager = new PluginManager();
            
            InitStaticVariables();

            InitConfig();

            LoadAssets();
            
            LoadSongsIntoSongList();

            InitializeSettings();

            //RunATestSong();
        }

        private void Update() {
            if (allowPause) {
                if (pauseButton.changed) {
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

        private void InitStaticVariables() {
            // set all static values
            SPAWN_DISTANCE = spawnDistance;
        
            AMBIGUOUS_MAT = ambiguousMaterial;
            LEFT_MAT = leftMaterial;
            RIGHT_MAT = rightMaterial;
            CENTER_MAT = centerMaterial;
        }

        private void InitConfig() {
            var logdir = Application.consoleLogPath.Substring(0, Application.consoleLogPath.Length - 10);
        
            // load config file / create if it doesn't exist already
            try {
                config = Config.Load(logdir);
                config.Save();
            }
            catch (Exception e) {
                Console.WriteLine(e);
                config = new Config();
                config.Save();
            }
            
        }

        private void LoadAssets() {
            foreach (var asset in includedAssetPackages) {
                pluginManager.AddPlugin(asset);
            }

            _currentGamemode = pluginManager.GetAllGamemodes()[0];
            LoadGamemode(_currentGamemode);
            SetEnvironment(pluginManager.GetAllEnvironments()[0]);
        }

        private void SetEnvironment(GameObject go) {
            _currentEnvironment = Instantiate(go);
        }

        private void LoadSongsIntoSongList() {
            List<Song> songs = new List<Song>();
            string[] paths = Directory.GetDirectories(config.songSavePath);
            foreach (var path in paths) {
                songs.Add(ReadSongFromPath(path + Path.DirectorySeparatorChar));
            }
            songList.AddRange(songs);
        }

        private void InitializeSettings() {
            settingsManager.settings.AddRange(integratedSettings);
            settingsManager.settingsMenuParent = uiManager.settingsMenu;
            settingsManager.UpdateSettingsUi();
        }

        private void RunATestSong() {
            // create debug beatmap
            var bm = new Beatmap();
            List<TrackingPoint[]> possibleTypes = new List<TrackingPoint[]>(); 
            possibleTypes.Add(new TrackingPoint[]{TrackingPoint.LeftFoot});
            possibleTypes.Add(new TrackingPoint[]{TrackingPoint.LeftHand});
            possibleTypes.Add(new TrackingPoint[]{TrackingPoint.Waist});
            possibleTypes.Add(new TrackingPoint[]{TrackingPoint.LeftFoot});
            possibleTypes.Add(new TrackingPoint[]{TrackingPoint.LeftFoot, TrackingPoint.LeftHand, TrackingPoint.RightHand, TrackingPoint.RightFoot, TrackingPoint.Waist});
            possibleTypes.Add(new TrackingPoint[]{TrackingPoint.RightFoot});
            List<Note> notes = new List<Note>();
            for (int i = 0; i < 240; i++) {
                var note = new Note();
                note.time = Random.Range((i-0.5f)/4f,(i+0.5f)/4f);
                note.cutDirection = i * 6;
                note.type = possibleTypes[(int) Math.Floor(Random.value * 6)];
                note.xPos = Random.value * 2 - 1;
                note.yPos = Random.value * 2;
                notes.Add(note);
            }

            bm.notes = notes.ToArray();
            // play beatmap
            StartCoroutine(PlayBeatmap(bm));
        }

        // return tag string for tracker role 
        private string TrackerRoleToTag(TrackingPoint role) {
            switch (role) {
                case TrackingPoint.LeftHand:
                    return "leftHand";
                case TrackingPoint.RightHand:
                    return "rightHand";
                case TrackingPoint.Waist:
                    return "waist";
                case TrackingPoint.LeftFoot:
                    return "leftFoot";
                case TrackingPoint.RightFoot:
                    return "rightFoot";
                default:
                    return null;
            }
        }

        // load a song
        public void LoadSong(string songpath) {
            var song = ReadSongFromPath(songpath);
            Debug.Log("Loaded Song " + song.songName + " by " + song.songAuthorName);
            songList.Add(song);
            uiManager.ListSongs(songList.GetAllSongs());
            //directly play the first beatmap from that song
            //Debug.Log("Playing Beatmap " + song.difficulties[0].name + " by " + song.difficulties[0].beatMapAuthor);
            //StartBeatmap(song, song.difficulties[0]);
        }

        public static Song ReadSongFromPath(string songpath) {
            var song = JsonUtility.FromJson<Song>(File.ReadAllText(songpath + "level.json"));
            song.pathToDir = songpath;
            return song;
        }

        // start the selected beatmap
        public void StartBeatmap(Song song, Difficulty difficulty, string modfiers) {
            Beatmap bm = JsonUtility.FromJson<Beatmap>(File.ReadAllText(config.songSavePath + Path.DirectorySeparatorChar + song.id + "_" + song.songName + Path.DirectorySeparatorChar + difficulty.beatMapPath));
            uiManager.InBeatmap();
            allowPause = true;
            currentlyPlayingBeatmap = bm;
            //StopCoroutine(PlayBeatmap(bm));
            StartCoroutine(PlayBeatmap(bm));
        }

        private IEnumerator PlayBeatmap(Beatmap bm) {
            //var beatmapLength = bm.notes[bm.notes.Length-1].time;
            float currentTime = 0;
            for (int i = 0; i < bm.notes.Length; i++) {
                var note = bm.notes[i];
                Debug.Log("Wait time: " + (note.time - currentTime));
                yield return new WaitForSeconds((note.time - currentTime));
                currentTime = note.time;
                Debug.Log("Current Time: " + currentTime);
                SpawnTarget(note.speed, note.xPos, note.yPos, note.cutDirection, note.rotation, note.type);
            }

            Debug.Log("Finished placing target objects");
        }

        public void StopBeatmap(int reason) {
            switch (reason) {
                case 0: // paused
                    Time.timeScale = 0;
                    uiManager.ShowPauseMenu(reason);
                    break;
                case 1: // completed
                    ExitBeatmap();
                    uiManager.ShowPauseMenu(reason);
                    break;
                case 2: // failed
                    ExitBeatmap();
                    uiManager.ShowPauseMenu(reason);
                    break;
                default:
                    break;
            }
        }

        public void ContinueBeatmap() {
            Time.timeScale = 1;
            uiManager.HidePauseMenu();
        }

        public void ExitBeatmap() {
            StopCoroutine(PlayBeatmap(currentlyPlayingBeatmap));
            allowPause = false;
            uiManager.ToSongListMenu();
            uiManager.HidePauseMenu();
        }

        // write song and all beatmaps to their files
        public string SaveSongToFile(Song songObject, Beatmap[] beatmaps, Texture2D cover) {
        public string SaveSongToFile(Song songObject, Beatmap[] beatmaps, Byte[] cover, Byte[] audio) {
            string pathToSong = config.songSavePath + songObject.id + "_" + songObject.songName.Replace("/", "") + Path.DirectorySeparatorChar;
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

    
        /*
     * loads a gamemode from assetbundle
     */
        public void LoadGamemode(Gamemode gm) {
            target = gm.targetObject;
            _currentTargetObject = target.GetComponent<TargetObject>();
            foreach (var trackedDevicePair in gm.trackedObjects) {
                trackedDevicePair.prefab.GetComponent<GenericTrackedObject>().collider.gameObject.tag = TrackerRoleToTag(trackedDevicePair.role);
                Transform tracker;
                if (trackedDevicePair.role == TrackingPoint.LeftHand) {
                    tracker = leftHand;
                } else if (trackedDevicePair.role == TrackingPoint.RightHand) {
                    tracker = rightHand;
                } else if (trackedDevicePair.role == TrackingPoint.LeftFoot) {
                    tracker = leftFoot;
                } else if (trackedDevicePair.role == TrackingPoint.RightFoot) {
                    tracker = rightFoot;
                } else if (trackedDevicePair.role == TrackingPoint.Waist) {
                    tracker = waist;
                } else {
                    return;
                }
                Instantiate(trackedDevicePair.prefab, tracker);
            }
        }
    
        // spawn a target object (switching target objects for gamemodes is still missing!)
        public void SpawnTarget(float speed, float xCoord, float yCoord, float viewRotation, float playspaceRoation, TrackingPoint[] hand) {
            GameObject cube = Instantiate(_currentTargetObject.gameObject, new Vector3(xCoord, yCoord, SPAWN_DISTANCE), new Quaternion(0, 0, viewRotation, 0));
            cube.GetComponent<TargetObject>().InitNote(new Note(1, xCoord, yCoord, speed, hand, viewRotation, playspaceRoation));
        }

        // spawn a obstacle ^ same here
        public void SpawnObstacle(float speed, float xCoord, float yCoord, float viewRotation, float playspaceRoation, float width, float height) {
        
        }
    }
}