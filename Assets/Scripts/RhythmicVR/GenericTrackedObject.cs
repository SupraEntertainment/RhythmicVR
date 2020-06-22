using UnityEngine;

namespace RhythmicVR {
    public class GenericTrackedObject : MonoBehaviour {
        public Collider collider;
        public TrackingPoint role;
    
        // Start is called before the first frame update
        private void Start() {
            collider.gameObject.AddComponent<Rigidbody>();
            CreateRigidbody(collider.gameObject.GetComponent<Rigidbody>());
        }

        private void CreateRigidbody(Rigidbody rb) {
            rb.useGravity = false;
            rb.constraints = RigidbodyConstraints.FreezeAll;
        }

        private void OnTriggerEnter(Collider other) {
            if (other.gameObject.GetComponent<TargetObject>() == null) return;
            if (other.gameObject.GetComponent<TargetObject>().MatchCollider(role)) {
                DetermineScore(other.gameObject);
            }
        }

        protected float DetermineScore(GameObject hitTarget) {
            Destroy(hitTarget);
            return 100;
        }
    }
}
