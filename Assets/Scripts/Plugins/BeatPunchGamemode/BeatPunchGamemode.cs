using RhythmicVR;
using UnityEngine;

namespace BeatPunchGamemode {
	public class BeatPunchGamemode : Gamemode {
    

		/// <summary>
		/// Spawn a Target Object
		/// </summary>
		/// <param name="noteData">The note data to use</param>
		public virtual void SpawnTarget(Note noteData) {
			noteData.xPos = noteData.xPos / 2;
			noteData.yPos = noteData.yPos / 2;
			GameObject cube = Instantiate(targetObject.gameObject, new Vector3(noteData.xPos, noteData.yPos, core.spawnDistance), new Quaternion(0, 0, noteData.cutDirection, 0));
			cube.GetComponent<TargetObject>().InitNote(noteData);
		}

		// spawn a obstacle ^ same here
		public virtual void SpawnObstacle(float speed, float xCoord, float yCoord, float viewRotation, float playspaceRoation, float width, float height) {
        
		}
	}
}