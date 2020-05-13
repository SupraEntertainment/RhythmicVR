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

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        SpawnCube(0.1f, Random.value *2 - 1, Random.value *2 - 1, Random.value *2 - 1, Random.value *2 - 1, (int)Math.Floor(Random.value *3));
    }

    public void SpawnCube(float speed, float xCoord, float yCoord, float viewRotation, float playspaceRoation, int hand) {
        GameObject cube = Instantiate(cutCube, new Vector3(xCoord, yCoord, spawnDistance), new Quaternion(0, 0, viewRotation, 0));
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
        cube.GetComponent<Rigidbody>().AddForce(new Vector3(0, 0, -speed / 1000));
    }

    public void SpawnWall(float speed, float xCoord, float yCoord, float viewRotation, float playspaceRoation, float width, float height) {
        
    }
}
