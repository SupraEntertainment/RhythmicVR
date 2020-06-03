using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour {

    public GameObject target;
    public static GameObject TARGET;
    public GameObject obstacle;
    public static GameObject OBSTACLE;
    
    public static Material AMBIGUOUS_MAT;
    public static Material LEFT_MAT;
    public static Material RIGHT_MAT;
    public static Material CENTER_MAT;
    public Material ambiguousMaterial;
    public Material leftMaterial;
    public Material rightMaterial;
    public Material centerMaterial;
    public float spawnDistance;
    public static float SPAWN_DISTANCE;
    private Config _config;

    // Start is called before the first frame update
    void Start() {
        TARGET = target;
        OBSTACLE = obstacle;

        SPAWN_DISTANCE = spawnDistance;
        
        AMBIGUOUS_MAT = ambiguousMaterial;
        LEFT_MAT = leftMaterial;
        RIGHT_MAT = rightMaterial;
        CENTER_MAT = centerMaterial;
        try {
            _config = JsonUtility.FromJson<Config>(File.ReadAllText(Application.dataPath));
        }
        catch (Exception e) {
            Console.WriteLine(e);
            _config = new Config();
        }
        
        var bm = new Beatmap();
        List<Note> notes = new List<Note>();
        for (int i = 0; i < 240; i++) {
            var note = new Note();
            note.time = Random.Range(i/4f-0.5f,i/4f+0.5f);
            note.rotation = i * 6;
            note.type = (int) Math.Floor(Random.value * 4);
            note.xPos = Random.value * 2 - 1;
            note.yPos = Random.value * 2;
            notes.Add(note);
        }

        Debug.Log("generated notes");

        bm.notes = notes.ToArray();
        StartCoroutine(PlayBeatmap(bm));
    }

    // Update is called once per frame
    private void FixedUpdate() {
        //SpawnTarget(1f, Random.value *2 - 1, Random.value *2, Random.value *360, /*Random.value *360*/ 0, );
    }

    public void SetTrackedObjects(TrackedDevicePair[] tdps) {
        foreach (var tdp in tdps) {
            tdp.prefab.GetComponent<GenericTrackedObject>().collider.gameObject.tag = TrackerRoleToTag(tdp.role);
        }
    }

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

    public void LoadSong(Song song) {
        
    }

    public void StartBeatmap(Song song, Difficulty difficulty) {
        Beatmap bm = JsonUtility.FromJson<Beatmap>(File.ReadAllText(_config.SongSavePath + "/" + song.id + "_" + song.songName + "/" + difficulty.beatMapPath));
        StartCoroutine(PlayBeatmap(bm));
    }

    private static IEnumerator PlayBeatmap(Beatmap bm) {
        //var beatmapLength = bm.notes[bm.notes.Length-1].time;
        float currentTime = 0;
        Debug.Log("in front of loop");
        for (int i = 0; i < bm.notes.Length; i++) {
            Debug.Log("in loop " + i);
            var note = bm.notes[i];
            yield return new WaitForSeconds(note.time - currentTime);
            currentTime = note.time;
            SpawnTarget(note.speed, note.xPos, note.yPos, note.cutDirection, note.rotation, note.type);
        }
    }

    public void SaveSongToFile(Song songObject, Beatmap[] beatmaps) {
        string pathToSong = _config.SongSavePath + "/" + songObject.id + "_" + songObject.songName + "/";
        File.WriteAllText(pathToSong + "level.json", JsonUtility.ToJson(songObject));
        for (var index = 0; index < beatmaps.Length; index++) {
            var beatmap = beatmaps[index];
            File.WriteAllText(pathToSong + songObject.difficulties[index].beatMapPath, JsonUtility.ToJson(beatmap));
        }
    }

    public static void SpawnTarget(float speed, float xCoord, float yCoord, float viewRotation, float playspaceRoation, int hand) {
        GameObject cube = Instantiate(TARGET, new Vector3(xCoord, yCoord, SPAWN_DISTANCE), new Quaternion(0, 0, viewRotation, 0));
        cube.GetComponent<TargetObject>().InitNote(new Note(1, xCoord, yCoord, speed, hand, viewRotation, playspaceRoation));
    }

    public static void SpawnObstacle(float speed, float xCoord, float yCoord, float viewRotation, float playspaceRoation, float width, float height) {
        
    }
}

internal class Config {
    public string SongSavePath { get; set; }

    public Config() {
        SongSavePath = Application.consoleLogPath;
    }
}
