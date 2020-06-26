using UnityEngine;

namespace RhythmicVR {
	/// <summary>
	/// stores info about gamemode such as the default Target Object,
	/// wich tracked devices to use and wich prefab to attach
	/// and how to manipulate the beatmap spanwing position scale, rotation
	/// </summary>
	public abstract class Gamemode : MonoBehaviour {
	
		public string name;
		public Sprite icon;

		[Tooltip("place the object here, that has the component \"TargetObject\" or a child class of it attatched")]
		public GameObject targetObject;
		public TrackedDevicePair[] trackedObjects;
		public SpaceMapping targetSpaceMapping; // translates json file positioning into unity coordinates (if needed) e.g. x=0, y=z z=y
	}
}