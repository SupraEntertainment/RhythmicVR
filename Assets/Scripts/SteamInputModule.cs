using Valve.VR;

public class SteamInputModule : VRInputModule
{
    
    public SteamVR_Input_Sources[] m_Sources = new SteamVR_Input_Sources[]{};
    public SteamVR_Action_Boolean m_Click = null;

    private int currentPointerId = 0;
    

    
    public override void Process() {
        base.Process();

        for (var i = 0; i < m_Sources.Length; i++) {
            // Press
            if (m_Click.GetStateDown(m_Sources[i])) {
                if (currentPointerId == i) {
                    Press();
                }
                else {
                    SetPointer(i);
                    currentPointerId = i;
                }
            }

            // Release
            if (m_Click.GetStateUp(m_Sources[i])) {
                Release();
            }
        }
    }
    
}
