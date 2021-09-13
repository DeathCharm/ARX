using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Overrides this object's localScale.
/// </summary>
public class ARX_Script_LocalScaleOverride : MonoBehaviour
{
    public Vector3 mvec_localScale = Vector3.one;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void FixedUpdate()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = mvec_localScale;
    }
}
