using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Activates the given list of Behaviours after a set amount of time has elepsed in Play Mode.
/// </summary>
public class ARX_Script_TimedActivator : MonoBehaviour
{
    public Behaviour[] moa_components;

    public float mnf_timeTillActive = 0.1F;

    public void SetTimer(float nfTimeTillActive)
    {
        mnf_timeTillActive = nfTimeTillActive;
        name += nfTimeTillActive;
    }

    void Activate()
    {
        foreach (Behaviour mono in moa_components)
        {
            mono.enabled = true;
        }
    }

    private void FixedUpdate()
    {
        float nfTimeElapsed = Time.deltaTime;
        
        if (mnf_timeTillActive > 0)
        {
            mnf_timeTillActive -= nfTimeElapsed;
            if (mnf_timeTillActive <= 0)
            {
                Activate();
                mnf_timeTillActive = 0;
                enabled = false;
            }
        }
    }
    
}
