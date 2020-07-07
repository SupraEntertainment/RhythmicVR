using Valve.VR;

public class SteamInputModule : VRInputModule {
    
    public SteamVR_Input_Sources[] sources;
    public SteamVR_Action_Boolean click;
    public SteamVR_Action_Vector2 scroll;

    private int currentPointerId = 0;
    
    public override void Process() {
        base.Process();

        for (var i = 0; i < sources.Length; i++) {
            // Press
            if (click.GetStateDown(sources[i])) {
                if (currentPointerId == i) {
                    Press();
                } else {
                    SetPointer(i);
                    currentPointerId = i;
                }
            }

            // Release
            if (click.GetStateUp(sources[i])) {
                Release();
            }
            
            // Scroll
            if (scroll.changed) {
                //Debug.Log("Scrolled " + m_Scroll.axis);
                Data.scrollDelta = scroll.axis * 20;
                Scroll();
            }
        }
    }
    
}
