using UnityEngine;

namespace RhythmicVR {
	public abstract class Gamemode : MonoBehaviour {
	
		public string name;
		public Sprite icon;

		[Tooltip("place the object here, that has the component \"TargetObject\" or a child of it attatched")]
		public GameObject targetObject;
		public TrackedDevicePair[] trackedObjects;
		public SpaceMapping targetSpaceMapping; // translates json file positioning into unity coordinates (if needed) e.g. x=0, y=z z=y
	}
}