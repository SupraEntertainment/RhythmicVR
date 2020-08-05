using RhythmicVR;
using UnityEngine;

namespace BlockSong {
	public class BlockSongTargetObject : TargetObject {
		private Note linkedData;
	
		/// <summary>
		/// Initialize note (set linked data, assign rotation, material, etc)
		/// </summary>
		/// <param name="data">note data</param>
		public void InitNote(Note data) {
			linkedData = data;
			GetComponent<MeshRenderer>().material = AssignMaterial(linkedData.target);
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