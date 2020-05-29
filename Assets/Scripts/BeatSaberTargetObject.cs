using UnityEngine;
public class BeatSaberTargetObject : TargetObject {
	
	
	public override void InitNote(Note data) {
		linkedData = data;
		switch (linkedData.type) {
			case 0:
				GetComponent<MeshRenderer>().material = GameManager.AMBIGUOUS_MAT;
				break;
			case 1:
				GetComponent<MeshRenderer>().material = GameManager.LEFT_MAT;
				break;
			case 2:
				GetComponent<MeshRenderer>().material = GameManager.RIGHT_MAT;
				break;
			case 3:
				GetComponent<MeshRenderer>().material = GameManager.CENTER_MAT;
				break;
		}
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