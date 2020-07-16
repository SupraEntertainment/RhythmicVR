using ScoreManager;
using UnityEngine;

namespace RhythmicVR {
	/// <summary>
	/// stores info about gamemode such as the default Target Object,
	/// wich tracked devices to use and wich prefab to attach
	/// and how to manipulate the beatmap spanwing position scale, rotation
	/// </summary>
	public class Gamemode : PluginBaseClass {
	
		public string gamemodeName;
		public Sprite icon;

		[Tooltip("place the object here, that has the component \"TargetObject\" or a child class of it attatched")]
		public GameObject targetObject;
		public Playthrough playthroughObject;
		public TrackedDevicePair[] trackedObjects;
		public SpaceMapping targetSpaceMapping; // translates json file positioning into unity coordinates (if needed) e.g. x=0, y=z z=y

		public override void Init(Core core) {
			base.Init(core);
			type = AssetType.Gamemode;
		}

		/// <summary>
		/// Spawn a Target Object
		/// </summary>
		/// <param name="noteData">The note data to use</param>
		public virtual void SpawnTarget(Note noteData) {
			GameObject cube = Instantiate(targetObject.gameObject, new Vector3(noteData.xPos, noteData.yPos, core.spawnDistance), new Quaternion(0, 0, noteData.cutDirection, 0));
			cube.GetComponent<TargetObject>().InitNote(noteData);
		}

		// spawn a obstacle ^ same here
		public virtual void SpawnObstacle(float speed, float xCoord, float yCoord, float viewRotation, float playspaceRoation, float width, float height) {
        
		}
	}
}