using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ARX;

/// <summary>
/// Extracts UnityEditor-ready code from a UI Image and its child UI Images into 
/// </summary>
[ExecuteInEditMode]
public class ARX_Script_CreateEditorWindow : MonoBehaviour
{
    public bool mb_run = false;

    public Image mo_img;

    [TextArea(3,25)]
    public string mstr_fullClassOutput = "";

    [TextArea(3, 15)]
    public string mstr_constants = "";
    
    [TextArea(3, 15)]
    public string mstr_drawfunctionoutput = "";

    [TextArea(3, 15)]
    public string mstr_onDrawFunction = "";


    private void Start()
    {
        if(mo_img == null)
            mo_img = GetComponent<Image>();
    }

    public void CreateFullClassOutput()
    {
        if (mo_img == null)
            mo_img = GetComponent<Image>();

        ARX_VariableSpecs specs = new ARX_VariableSpecs();
        specs.mstr_name = mo_img.name.TrimWhitespace();
        specs.me_output = ARX.VarGen.VariableGenOutputType.Class;
        specs.mb_isStatic = false;
        specs.me_security = ARX.VarGen.SecurityScope.Public;
        specs.GetParentClasses.Add("EditorWindow");
        specs.mstr_content = mstr_constants + "\n\n" + mstr_drawfunctionoutput + "\n\n";

        string strUsing = "using UnityEditor;\nusing UnityEngine;\n\n";

        mstr_fullClassOutput = strUsing + specs.GetOutput();
    }

    private void Update()
    {
        if(mb_run && mo_img != null)
        {
            mb_run = false;
            mstr_constants = "";
            mstr_onDrawFunction = "";
            mstr_drawfunctionoutput = "";
            ARX_ImageToEditorWindow nester = new ARX_ImageToEditorWindow(mo_img, this);
            nester.Run(mo_img);

            CreateFullClassOutput();
        }
    }

}
