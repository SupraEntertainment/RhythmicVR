using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour {

    public GameObject cutCube;
    public GameObject wall;
    public Material leftMaterial;
    public Material rightMaterial;
    public Material centerMaterial;
    public float spawnDistance;
    public float speed = 0.1f;

    // Start is called before the first frame update
    void Start() {
    }

    // Update is called once per frame
    void FixedUpdate() {
        SpawnCube(speed, Random.value *2 - 1, /*Random.value *2 - 1*/ 0, Random.value *360, Random.value *360, (int)Math.Floor(Random.value *3));
    }

    public void SpawnCube(float speed, float xCoord, float yCoord, float viewRotation, float playspaceRoation, int hand) {
        GameObject cube = Instantiate(cutCube, new Vector3(xCoord, yCoord, spawnDistance), new Quaternion(0, 0, viewRotation, 0));
        cube.transform.RotateAround(transform.position, Vector3.up, playspaceRoation);
        switch (hand) {
            case 0:
                cube.GetComponent<MeshRenderer>().material = leftMaterial;
                break;
            case 1:
                cube.GetComponent<MeshRenderer>().material = rightMaterial;
                break;
            case 2:
                cube.GetComponent<MeshRenderer>().material = centerMaterial;
                break;
        }
    }

    public void SpawnWall(float speed, float xCoord, float yCoord, float viewRotation, float playspaceRoation, float width, float height) {
        
    }
}
