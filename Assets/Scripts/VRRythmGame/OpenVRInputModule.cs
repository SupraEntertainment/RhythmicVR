using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Valve.VR;

namespace VRRythmGame {
	public class OpenVRInputModule : BaseInputModule {

		public Camera camera;
		public SteamVR_Input_Sources sources;
		public SteamVR_Action_Boolean menuClick;
		
		private GameObject _currentObject = null;
		private PointerEventData _pointerEventData = null;


		protected override void Awake() {
			base.Awake(); 
			
			_pointerEventData = new PointerEventData(eventSystem);
		}

		public PointerEventData GetData() {
			return _pointerEventData;
		}


		//-------------------------------------------------
		public override bool ShouldActivateModule()
		{
			if ( !base.ShouldActivateModule() )
				return false;

			return _currentObject != null;
		}


		//-------------------------------------------------
		public void ProcessPress(PointerEventData data)
		{
			// Set Raycast
			data.pointerPressRaycast = data.pointerCurrentRaycast;

			// Check Object hit, get down handler, call
			GameObject newPointerPress = ExecuteEvents.ExecuteHierarchy(_currentObject, data, ExecuteEvents.pointerDownHandler);

			// if no down handler, get click handler
			if (!newPointerPress) {
				newPointerPress = ExecuteEvents.GetEventHandler<IPointerClickHandler>(_currentObject);
			}

			// set data
			data.pressPosition = data.position;
			data.pointerPress = data.rawPointerPress;
		}


		//-------------------------------------------------
		public void ProcessRelease(PointerEventData data) {
			ExecuteEvents.Execute(data.pointerPress, data, ExecuteEvents.pointerUpHandler);

			GameObject pointerUpHandler = ExecuteEvents.GetEventHandler<IPointerClickHandler>(_currentObject);

			if (data.pointerPress == pointerUpHandler) {
				ExecuteEvents.Execute(data.pointerPress, data, ExecuteEvents.pointerClickHandler);
			}

			eventSystem.SetSelectedGameObject(null);
			
			data.pressPosition = Vector2.zero;
			data.pointerPress = null;
			data.rawPointerPress = null;
		}


		//-------------------------------------------------
		public void HoverBegin( GameObject gameObject )
		{
			PointerEventData pointerEventData = new PointerEventData( eventSystem );
			ExecuteEvents.Execute( gameObject, pointerEventData, ExecuteEvents.pointerEnterHandler );
		}


		//-------------------------------------------------
		public void HoverEnd( GameObject gameObject )
		{
			PointerEventData pointerEventData = new PointerEventData( eventSystem );
			pointerEventData.selectedObject = null;
			ExecuteEvents.Execute( gameObject, pointerEventData, ExecuteEvents.pointerExitHandler );
		}


		//-------------------------------------------------
		public override void Process() {
			// Camera
			_pointerEventData.Reset();
			_pointerEventData.position = new Vector2(camera.pixelWidth/2, camera.pixelHeight/2);
			
			// Raycast
			eventSystem.RaycastAll(_pointerEventData, m_RaycastResultCache);
			_pointerEventData.pointerCurrentRaycast = FindFirstRaycast(m_RaycastResultCache);
			_currentObject = _pointerEventData.pointerCurrentRaycast.gameObject;
			
			// Clear
			m_RaycastResultCache.Clear();
			
			HandlePointerExitAndEnter(_pointerEventData, _currentObject);
			
			
			// Click
			if (menuClick.GetStateDown(sources)) {
				ProcessPress(_pointerEventData);
			}
			
			// Release
			if (menuClick.GetStateUp(sources)) {
				ProcessRelease(_pointerEventData);
			}
		}
	}
}