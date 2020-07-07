using RhythmicVR;
using UnityEngine;

namespace BeatSaber {
	public class BeatSaberTargetObject : TargetObject {
	
		/// <summary>
		/// Initialize note (set linked data, assign rotation, material, etc)
		/// </summary>
		/// <param name="data">note data</param>
		public override void InitNote(Note data) {
			linkedData = data;
			GetComponent<MeshRenderer>().material = AssignMaterial(linkedData.type);
			transform.RotateAround(new Vector3(0, 0, 0), Vector3.up, linkedData.rotation);
			transform.Rotate(transform.forward, linkedData.cutDirection);
			isInitialized = true;
		}

		private void FixedUpdate()
		{
			if (isInitialized) {
				transform.transform.Translate(0, 0, -linkedData.speed * 0.1f, Space.Self);
			}
		}
	}
}