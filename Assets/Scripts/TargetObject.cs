using UnityEngine;

public abstract class TargetObject : MonoBehaviour {
	protected Note linkedData = new Note();
	protected bool isInitialized;

	public abstract void InitNote(Note data);

	private void OnTriggerExit(Collider other) {
		if (other.gameObject.CompareTag("despawn")) {
			Destroy(gameObject);
		}
	}
}