using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Valve.VR;

namespace VRRythmGame {
    public class RaycastInteractor : MonoBehaviour {

        public Vector3 direction;
        public SteamVR_Action_Boolean menuClick;
        public GameObject canvas;
        public Plane uiPanel;
        public GameObject uiDotPrefab;
        private GameObject _uiDot;
        PointerEventData m_PointerEventData;
        EventSystem m_EventSystem;

        void Start()
        {
            
            uiPanel = new Plane(canvas.transform.forward, -canvas.transform.position.z);
            //Fetch the Event System from the Scene
            m_EventSystem = GetComponent<EventSystem>();
        }

        void Update() {
            Ray ray = new Ray(new Vector3(0, 0, 1), transform.forward);
            Debug.DrawRay(transform.position, transform.forward, Color.magenta);
            
            float enter;

            if (uiPanel.Raycast(ray, out enter)) {
                Vector3 hit = ray.GetPoint(enter);
                Debug.Log("hit?");

                if (_uiDot == null) {
                    _uiDot = Instantiate(uiDotPrefab, hit, Quaternion.Euler(uiPanel.normal));
                } else {
                    _uiDot.transform.position = hit;
                }
                
                //send hit to the canvas
                //Set up the new Pointer Event
                m_PointerEventData = new PointerEventData(m_EventSystem);
                //Set the Pointer Event Position to that of the mouse position
                m_PointerEventData.position = hit;
                
                
                
                if (menuClick.state) {
                    //do the click
                    Debug.Log("HELP!");
                }
            }
            else {
                Destroy(_uiDot);
            }
            
            
            //Check if the left Mouse button is clicked
            if (Input.GetKey(KeyCode.Mouse0))
            {
            }
        }
    }
}
