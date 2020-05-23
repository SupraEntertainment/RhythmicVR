using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour {

    public GameObject target;
    public GameObject obstacle;
    public static Material AMBIGUOUS_MAT;
    public static Material LEFT_MAT;
    public static Material RIGHT_MAT;
    public static Material CENTER_MAT;
    public Material ambiguousMaterial;
    public Material leftMaterial;
    public Material rightMaterial;
    public Material centerMaterial;
    public float spawnDistance;

    // Start is called before the first frame update
    void Start() {
        AMBIGUOUS_MAT = ambiguousMaterial;
        LEFT_MAT = leftMaterial;
        RIGHT_MAT = rightMaterial;
        CENTER_MAT = centerMaterial;
    }

    // Update is called once per frame
    private void FixedUpdate() {
        SpawnTarget(Random.value *2 - 1, Random.value *2, Random.value *360, /*Random.value *360*/ 0, (int)Math.Floor(Random.value *4));
    }

    public void SpawnTarget(float xCoord, float yCoord, float viewRotation, float playspaceRoation, int hand) {
        GameObject cube = Instantiate(target, new Vector3(xCoord, yCoord, spawnDistance), new Quaternion(0, 0, viewRotation, 0));
        cube.GetComponent<NoteObject>().InitNote(new Note(1, xCoord, yCoord, 0.1f, hand, viewRotation, playspaceRoation));
    }

    public void SpawnObstacle(float speed, float xCoord, float yCoord, float viewRotation, float playspaceRoation, float width, float height) {
        
    }
}
