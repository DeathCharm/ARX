using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Freezes this object's rotation to a given value.
/// </summary>
[ExecuteInEditMode]
public class ARX_Script_OverrideRotation : MonoBehaviour
{
    public Vector3 vecRotation;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.eulerAngles = vecRotation;
    }
}
