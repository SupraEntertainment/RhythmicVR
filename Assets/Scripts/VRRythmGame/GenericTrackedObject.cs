using UnityEngine;

namespace VRRythmGame {
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
            if (other.gameObject.GetComponent<TargetObject>() != null) {
                if (true) { //other.gameObject.GetComponent<TargetObject>().MatchCollider(role)) {
                    Destroy(other.gameObject);
                }
            }
        }
    }
}
