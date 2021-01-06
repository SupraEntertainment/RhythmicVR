
using System.Windows.Forms.VisualStyles;
using UnityEngine;
using UnityEngine.InputSystem;

public class OpenXRInputModule : VRInputModule, Default.IUIActions {

	// UI
	Default controls;
	public InputActionAsset asset;
	private InputActionMap m_UI;
	//private IUIActions m_UIActionsCallbackInterface;
	private InputAction m_UI_click_ui;
	private InputAction m_UI_scroll;

    private int currentPointerId = 0;

    protected override void Start() {
	    m_UI = asset.FindActionMap("UI", throwIfNotFound: true);
	    m_UI_click_ui = m_UI.FindAction("click_ui", throwIfNotFound: true);
	    m_UI_scroll = m_UI.FindAction("scroll", throwIfNotFound: true);
    }

    protected override void OnEnable() {
	    if (controls == null) {
		    controls = new Default();
		    
		    controls.UI.SetCallbacks(this);
	    }
    }

    public override void Process() {
        base.Process();
		/*
        for (var i = 0; i < m_UI.devices.Value.Count; i++) {
            // Press
            if (m_UI_click_ui.triggered) {
                if (currentPointerId == i) {
                    Press();
                } else {
                    SetPointer(i);
                    currentPointerId = i;
                }
            }

            // Release
            if (m_UI_click_ui.triggered) {
                Release();
            }
            
            // Scroll
            if (m_UI_scroll.phase == InputActionPhase.Started) {
                //Debug.Log("Scrolled " + m_Scroll.axis);
                Data.scrollDelta = m_UI_scroll.controls[0].valueSizeInBytes * 20;
                Scroll();
            }
        }*/
    }

    public void OnClick_ui(InputAction.CallbackContext context) {
	    // Press
	    if (context.performed) {
		    if (currentPointerId == 0) {
			    Press();
		    } else {
			    SetPointer(0);
			    currentPointerId = 0;
		    }
	    }

	    // Release
	    if (context.performed) {
		    Release();
	    }
    }

    public void OnScroll(InputAction.CallbackContext context) {
	    Data.scrollDelta = context.ReadValue<Vector2>() * 20;
	    Scroll();
    }
}
