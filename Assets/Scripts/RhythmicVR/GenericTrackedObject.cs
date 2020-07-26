using ScoreManager;
using UnityEngine;

namespace RhythmicVR {
    /// <summary>
    /// Object to use on tracked devices, determines hit and score logic aswell as hit collider
    /// </summary>
    public class GenericTrackedObject : MonoBehaviour {
        public Collider mCollider;
        public bool useVelocityForScoreCalc;
        [System.NonSerialized] public TrackingPoint role;
        public Core core;
        private ScoreManager.ScoreManager scoreManager;
    
        // Start is called before the first frame update
        private void Start() {
            mCollider.gameObject.AddComponent<Rigidbody>();
            CreateRigidbody(mCollider.gameObject.GetComponent<Rigidbody>());
            core = FindObjectOfType<Core>();
            scoreManager = (ScoreManager.ScoreManager)core.pluginManager.Find("score_manager");
        }

        private void CreateRigidbody(Rigidbody rb) {
            rb.useGravity = false;
            rb.constraints = RigidbodyConstraints.FreezeAll;
        }

        private void OnTriggerEnter(Collider other) {
            if (other.gameObject.GetComponent<TargetObject>() == null) return;
            if (other.gameObject.GetComponent<TargetObject>().MatchCollider(role)) {
                scoreManager.currentPlaythrough.AddScore(DetermineScore(other.gameObject));
            } else {
                scoreManager.currentPlaythrough.AddScore(0);
            }
            Destroy(other.gameObject);
        }

        protected virtual float DetermineScore(GameObject hitTarget) {
            float score = 100;
            if (useVelocityForScoreCalc) {
                score = GetComponent<Rigidbody>().velocity.magnitude * 10;
            }
            return score;
        }
    }
}
