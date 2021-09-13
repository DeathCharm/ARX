using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ARX;

[ExecuteInEditMode]
public class ARX_Script_VariableGen : MonoBehaviour
{
    public bool bPause = false;

    public ARX_VariableSpecs specs;
    
    [TextArea(3, 10)]
    public string mstr_output = "";

    void CreateOutputString()
    {
        mstr_output = specs.GetOutput();
    }

    // Update is called once per frame
    void Update()
    {
        if (bPause == false)
        {
            CreateOutputString();
        }
    }
}
