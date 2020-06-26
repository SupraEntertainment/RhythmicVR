using UnityEngine;
using UnityEngine.SceneManagement;

namespace RhythmicVR {
    /// <summary>
    /// Object to use on tracked devices, determines hit and score logic aswell as hit collider
    /// </summary>
    public class GenericTrackedObject : MonoBehaviour {
        public Collider collider;
        public TrackingPoint role;
        public GameManager gm;
    
        // Start is called before the first frame update
        private void Start() {
            collider.gameObject.AddComponent<Rigidbody>();
            CreateRigidbody(collider.gameObject.GetComponent<Rigidbody>());
            gm = FindObjectOfType<GameManager>();
        }

        private void CreateRigidbody(Rigidbody rb) {
            rb.useGravity = false;
            rb.constraints = RigidbodyConstraints.FreezeAll;
        }

        private void OnTriggerEnter(Collider other) {
            if (other.gameObject.GetComponent<TargetObject>() == null) return;
            if (other.gameObject.GetComponent<TargetObject>().MatchCollider(role)) {
                gm.currentScore = DetermineScore(other.gameObject);
            }
        }

        protected float DetermineScore(GameObject hitTarget) {
            Destroy(hitTarget);
            return 100;
        }
    }
}
