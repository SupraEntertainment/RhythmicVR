using UnityEngine;

namespace VRRythmGame {
	public abstract class TargetObject : MonoBehaviour {
		protected Note linkedData = new Note();
		protected bool isInitialized;

		public abstract void InitNote(Note data);

		public bool MatchCollider(TrackingPoint tp) {
			foreach (var trackingPoint in linkedData.type) {
				if (trackingPoint == tp) {
					return true;
				}
			}
			return false;
		}

		public Material AssignMaterial(TrackingPoint[] trackingPoints) {
			if (trackingPoints.Length > 1) {
				return GameManager.AMBIGUOUS_MAT;
			} else {
				switch (trackingPoints[0]) {
					case TrackingPoint.LeftFoot:
					case TrackingPoint.LeftHand:
						return GameManager.LEFT_MAT;
					case TrackingPoint.RightFoot:
					case TrackingPoint.RightHand:
						return GameManager.RIGHT_MAT;
					case TrackingPoint.Waist:
						return GameManager.CENTER_MAT;
					default:
						return GameManager.AMBIGUOUS_MAT;
				}
			}
		}

		private void OnTriggerExit(Collider other) {
			if (other.gameObject.CompareTag("despawn")) {
				Destroy(gameObject);
			}
		}
	}
}