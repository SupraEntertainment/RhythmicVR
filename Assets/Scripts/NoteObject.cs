using UnityEngine;

public class NoteObject : MonoBehaviour {
	private Note _linkedData = new Note();
	private bool _isInitialized;

	public void InitNote(Note data) {
		_linkedData = data;
		switch (_linkedData.type) {
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
		transform.RotateAround(new Vector3(0, 0, 0), Vector3.up, _linkedData.rotation);
		transform.Rotate(transform.forward, _linkedData.cutDirection);
		_isInitialized = true;
	}

	private void FixedUpdate()
	{
		if (_isInitialized) {
			transform.transform.Translate(0, 0, -_linkedData.speed * 0.1f, Space.Self);
		}
	}

	private void OnTriggerExit(Collider other) {
		if (other.gameObject.CompareTag("despawn")) {
			Destroy(gameObject);
		}
	}
}