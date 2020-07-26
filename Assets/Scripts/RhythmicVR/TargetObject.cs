using UnityEngine;

namespace RhythmicVR {
	public abstract class TargetObject : MonoBehaviour {
		protected Note linkedData = new Note();
		protected bool isInitialized;
		public bool shouldDespawnBehindPlayer = true;

		/// <summary>
		/// Initialize note (set linked data, assign rotation, material, etc)
		/// </summary>
		/// <param name="data">note data</param>
		public abstract void InitNote(Note data);

		/// <summary>
		/// look if it should be hit by the tracking point it got hit with (rest of logic is done on gamemode side (GenericTrackedOBject)
		/// </summary>
		/// <param name="tp">The tracking point to match with</param>
		/// <returns>successful or not</returns>
		public bool MatchCollider(TrackingPoint tp) {
			foreach (var trackingPoint in linkedData.type) {
				if (trackingPoint == tp) {
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Return material based on tracking points it should be hitable with
		/// </summary>
		/// <param name="trackingPoints">The tracking points, that cna hit it</param>
		/// <returns>The material to assign</returns>
		public Material AssignMaterial(TrackingPoint[] trackingPoints) {
			if (trackingPoints.Length > 1) {
				return Core.AMBIGUOUS_MAT;
			} else {
				switch (trackingPoints[0]) {
					case TrackingPoint.LeftFoot:
					case TrackingPoint.LeftHand:
						return Core.LEFT_MAT;
					case TrackingPoint.RightFoot:
					case TrackingPoint.RightHand:
						return Core.RIGHT_MAT;
					case TrackingPoint.Waist:
						return Core.CENTER_MAT;
					default:
						return Core.AMBIGUOUS_MAT;
				}
			}
		}

		private void OnTriggerExit(Collider other) {
			if (other.gameObject.CompareTag("despawn")) {
				if (shouldDespawnBehindPlayer) {
					((ScoreManager.ScoreManager)FindObjectOfType<Core>().pluginManager.Find("score_manager")).currentPlaythrough.AddScore(0);
					Destroy(gameObject);
				}
			}
		}
	}
}