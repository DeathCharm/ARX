using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script causes gameobject to SetActive(false) on Start.
/// Can also be set to run one single time, then deactivate.
/// </summary>
public class ARX_Script_DeactivateSelf : MonoBehaviour
{
    /// <summary>
    /// Allow the object to run once before disabling it?
    /// </summary>
    public bool mb_runOnce = false;

    // Start is called before the first frame update
    void Start()
    {
        if(mb_runOnce == false)
            gameObject.SetActive(false);
    }

    private void Update()
    {
        gameObject.SetActive(false);
    }

}
