using UnityEngine;
using UnityEngine.EventSystems;

namespace RhythmicVR {
    public class RaycastInteractor : MonoBehaviour {

        public float defaultLength = 5.0f;
        public GameObject uiDotPrefab;
        public OpenVRInputModule inputModule;
        
        private LineRenderer _lineRenderer = null;
        private GameObject _uiDot;

        private void Awake() {
            _lineRenderer = GetComponent<LineRenderer>();
        }

        void Update() {
            UpdateLine();
        }

        private void UpdateLine() {
            // get length
            PointerEventData data = inputModule.GetData();
            float targetlength = data.pointerCurrentRaycast.distance == 0 ? defaultLength : data.pointerCurrentRaycast.distance;

            RaycastHit hit = CreateRaycast(targetlength);
            
            Vector3 hitPoint = transform.position + (transform.forward * targetlength);

            if (hit.collider != null) {

                hitPoint = hit.point;

                if (_uiDot == null) {
                    _uiDot = Instantiate(uiDotPrefab, hitPoint, Quaternion.Euler(hit.collider.gameObject.transform.forward));
                } else {
                    _uiDot.transform.position = hitPoint;
                }
            }
            else {
                Destroy(_uiDot);
            }
            
            _lineRenderer.SetPosition(0, transform.position);
            _lineRenderer.SetPosition(0, hitPoint);
        }

        private RaycastHit CreateRaycast(float length) {
            RaycastHit hit = new RaycastHit();
            Ray ray = new Ray(transform.position, transform.forward);

            Physics.Raycast(ray, out hit, defaultLength);
            return hit;
        }
    }
}
