using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ARX;
using FEN;

/// <summary>
/// Simple, constant process made to test Top Process functionality
/// </summary>
public class ARX_Process_Metronome : ARX_Process
{

    ARX_Script_IndividualColor mo_colorScript = null;
    public GameObject mo_cube;
    public ARX_Process_Metronome():base("metronome")
    {
        CreateMetronomeCube();
    }

    void CreateMetronomeCube()
    {
        mo_cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        mo_cube.transform.localScale = new Vector3(1, 5, 1);
        mo_colorScript = mo_cube.AddComponent<ARX_Script_IndividualColor>();

    }

    public override void OnFixedUpdate()
    {
        Vector3 vecRotateSpeed = new Vector3(0, 0, 1);
        mo_cube.transform.Rotate(vecRotateSpeed);
        mo_colorScript.mo_color = Color.green;
    }

    public override bool OnInactiveFixedUpdate()
    {
        mo_colorScript.mo_color = Color.red;
        return base.OnInactiveFixedUpdate();
    }

}
