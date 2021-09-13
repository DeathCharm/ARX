using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script calls DontDestroyOnLoad on its gameobject on Start().
/// </summary>
public class ARX_Script_DontDestroyOnLoad : MonoBehaviour {

	// Use this for initialization
	void Start () {
        DontDestroyOnLoad(gameObject);
	}
	
}
