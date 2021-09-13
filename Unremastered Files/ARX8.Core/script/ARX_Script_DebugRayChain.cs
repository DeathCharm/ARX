using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class uses Debug.DrawLine to draw a line from this object to the mo_target object.
/// </summary>
[ExecuteInEditMode]
public class ARX_Script_DebugRayChain : MonoBehaviour {
    public GameObject mo_target;
    public Color mo_color = Color.green;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(mo_target == null)
            return;
        
        Debug.DrawLine(transform.position, mo_target.transform.position, mo_color);


	}
}
