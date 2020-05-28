using System;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public abstract class Gamemode : MonoBehaviour {

    public TrackedDevicePair[] trackedObjects;
    protected GameManager _gm;

    private void Start() {
	    _gm = FindObjectOfType<GameManager>();
	    _gm.SetTrackedObjects(trackedObjects);
    }
}
