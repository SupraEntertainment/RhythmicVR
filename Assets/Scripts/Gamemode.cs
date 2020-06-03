using UnityEngine;

public abstract class Gamemode : MonoBehaviour {

    public TrackedDevicePair[] trackedObjects;
    protected GameManager gm;
    public SpaceMapping targetSpaceMapping; // translates json file positioning into unity coordinates (if needed) e.g. x=0, y=z z=y

    private void Start() {
	    gm = FindObjectOfType<GameManager>();
	    gm.SetTrackedObjects(trackedObjects);
    }
}