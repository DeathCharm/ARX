using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ARX;

/// <summary>
/// Script holding an ARX_StatQuadBox for debugging in Edit Mode
/// </summary>
[ExecuteInEditMode]
public class ARX_Script_StatBox : MonoBehaviour {

    
    public ARX_StatBox_Quad mo_stats = null;

    void Awake()
    {
        if (mo_stats == null)
            mo_stats = ScriptableObject.CreateInstance<ARX_StatBox_Quad>();
    }

    void Start()
    {
        if (mo_stats == null)
            mo_stats = ScriptableObject.CreateInstance<ARX_StatBox_Quad>();
    }


}
